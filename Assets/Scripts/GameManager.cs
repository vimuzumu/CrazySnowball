using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    private const float EXP_PER_SIZE_MODIFIER = 200f;

    public static int currentCoinsAmount;

    private static GameManager gameManager;

    private Rigidbody snowball;
    private bool startedLevelEnd;
    private bool finishedLevelEnd;
    private int currentSize;
    private float currentExp;
    private float levelEndBonus;
    private CameraController cameraController;
    private Magnet magnet;

    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private TextMeshProUGUI instructionText;
    [SerializeField]
    private GameObject levelEndBlock;
    [SerializeField]
    private ParticleSystem snowEmitter;

    private void Awake()
    {
        gameManager = this;
        snowball = GameObject.Find("Snowball").GetComponent<Rigidbody>();
        cameraController = Camera.main.GetComponent<CameraController>();
        magnet = snowball.GetComponentInChildren<Magnet>();
        currentSize = 1;
        currentExp = 0f;
        currentCoinsAmount = 0;
        levelEndBonus = 0f;
        GetComponent<LevelEndScript>().BuildEndOfLevel(GetLevelEndPosition(), levelEndBlock.transform);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static GameManager GetGameManager()
    {
        return gameManager;
    }

    public Rigidbody GetSnowball()
    {
        return snowball;
    }

    public Vector3 GetLevelEndPosition()
    {
        return levelEndBlock.transform.position;
    }

    public void StartLevelEnd()
    {
        if (!startedLevelEnd) {
            instructionText.text = "Tap Fast";
            cameraController.PlaySpeedEffect(100);
        }
        startedLevelEnd = true;
    }

    public bool IsStartedLevelEnd()
    {
        return startedLevelEnd;
    }

    public void FinishLevelEnd()
    {
        instructionText.text = "";
        finishedLevelEnd = true;
    }

    public bool IsFinishedLevelEnd()
    {
        return finishedLevelEnd;
    }

    public void GainExp(float expAmount, bool isLevelEnd = false)
    {
        currentExp += expAmount;
        float expRequired = GetExpForNextSize();
        if (currentExp > expRequired)
        {
            currentSize++;
            currentExp -= expRequired;
            snowball.transform.localScale += Vector3.one;
        }
        if (isLevelEnd)
        {
            levelEndBonus += expAmount;
            instructionText.text = "Bonus - " + levelEndBonus;
        }
    }

    public void LoseExp(float expAmount)
    {
        currentExp -= expAmount;
        if (currentExp < 0)
        {
            currentExp = 0;
        }
        snowEmitter.Play();
    }

    public float GetExpForNextSize()
    {
        return currentSize * EXP_PER_SIZE_MODIFIER;
    }

    public int GetCurrentSize()
    {
        return currentSize;
    }

    public float GetCurrentExp()
    {
        return currentExp;
    }

    public Canvas GetCanvas()
    {
        return canvas;
    }

    public void Fever()
    {
        instructionText.text = "FEVER";
        magnet.Enable();
        cameraController.PlaySpeedEffect(70);
        snowball.AddForce(Vector3.forward * 5f);
    }

    public void FeverDone()
    {
        instructionText.text = "";
        magnet.Enable(false);
    }
}
