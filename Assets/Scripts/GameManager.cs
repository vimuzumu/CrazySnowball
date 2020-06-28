using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const float EXP_PER_SIZE_MODIFIER = 200f;

    private static GameManager gameManager;

    private Rigidbody snowball;
    private bool startedLevelEnd;
    private bool jumpedLevelEnd;
    private bool collidedLevelEnd;
    private int currentSize;
    private float currentExp;

    [SerializeField]
    private GameObject LevelEndBlock;

    private void Awake()
    {
        gameManager = this;
        snowball = GameObject.Find("Snowball").GetComponent<Rigidbody>();
        currentSize = 1;
        currentExp = 0f;
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
        return LevelEndBlock.transform.position;
    }

    public void StartLevelEnd()
    {
        startedLevelEnd = true;
    }

    public bool IsStartedLevelEnd()
    {
        return startedLevelEnd;
    }

    public void JumpLevelEnd()
    {
        jumpedLevelEnd = true;
    }

    public bool IsJumpedLevelEnd()
    {
        return jumpedLevelEnd;
    }

    public void CollideLevelEnd()
    {
        collidedLevelEnd = true;
    }

    public bool IsCollidedLevelEnd()
    {
        return collidedLevelEnd;
    }

    public void GainExp(float expAmount)
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
}
