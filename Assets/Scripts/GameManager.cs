using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    private const string CURRENT_COINS_KEY = "CurrentCoins";

    public static int currentCoinsAmount;

    private static GameManager gameManager;

    private Rigidbody snowball;
    private bool startedLevelEnd;
    private bool finishedLevelEnd;
    private int currentSize;
    private float currentExp;
    private float tapEffect;
    private float levelEndBonus;
    private CameraController cameraController;
    private bool gameStarted;
    private bool gameRunning;

    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private TextMeshProUGUI instructionText;
    [SerializeField]
    private GameObject tapImage;
    [SerializeField]
    private GameObject levelEndGauge;
    [SerializeField]
    private GameObject levelEndBlock;
    [SerializeField]
    private ParticleSystem snowEmitter;
    [SerializeField]
    private Magnet magnet;

    private void Awake()
    {
        gameManager = this;
        snowball = GameObject.Find("Snowball").GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        currentSize = 1;
        currentExp = 0f;
        currentCoinsAmount = PlayerPrefs.GetInt(CURRENT_COINS_KEY);
        levelEndBonus = 0f;
        GetComponent<LevelEndScript>().BuildEndOfLevel(GetLevelEndPosition(), levelEndBlock.transform);
        tapEffect = 50f;
        gameRunning = true;
        gameStarted = false;
        instructionText.text = "Hold to move";
    }

    public static GameManager GetGameManager()
    {
        return gameManager;
    }

    public Rigidbody GetSnowball()
    {
        return snowball;
    }

    public void StartGame()
    {
        gameStarted = true;
        instructionText.text = "";
    }

    public Vector3 GetLevelEndPosition()
    {
        return levelEndBlock.transform.position;
    }

    private IEnumerator LevelEndUI()
    {
        tapImage.SetActive(true);
        levelEndGauge.SetActive(true);
        RectTransform transform = tapImage.GetComponent<RectTransform>();
        bool flip = true;
        while (!finishedLevelEnd)
        {
            transform.localScale = Vector3.one * (flip ? 2.8f : 3f);
            flip = !flip;
            yield return new WaitForSeconds(0.06f);
        }
        yield return null;
    }

    private IEnumerator FinishLevelEndIn2Seconds()
    {
        yield return new WaitForSeconds(2f);
        FinishLevelEnd();
    }

    public void StartLevelEnd()
    {
        if (!startedLevelEnd) {
            StartCoroutine(LevelEndUI());
            instructionText.text = "Tap Fast";
            cameraController.PlaySpeedEffect(100);
            StartCoroutine(FinishLevelEndIn2Seconds());
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
        tapImage.SetActive(false);
        levelEndGauge.SetActive(false);
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
        return currentSize * Settings.expPerSizeMultiplier;
    }

    public int GetCurrentSize()
    {
        return currentSize;
    }

    public float GetCurrentExp()
    {
        return currentExp;
    }

    public float GetCurrentTapEffect()
    {
        return tapEffect;
    }

    public void IncTapEffect()
    {
        tapEffect = Mathf.Clamp(tapEffect * 1.25f, tapEffect + 10f, 100f);
    }

    public void DecTapEffect()
    {
        tapEffect = Mathf.Clamp(tapEffect - 1, 0f, 100f);
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

    public bool IsGameRunning()
    {
        return gameRunning && gameStarted;
    }

    public void SetGameRunning(bool run)
    {
        gameRunning = run;
    }

    public bool DidGameStart()
    {
        return gameStarted;
    }
}
