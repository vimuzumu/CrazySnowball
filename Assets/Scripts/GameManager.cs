using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const float EXP_PER_SIZE_MODIFIER = 200f;

    private static GameManager gameManager;

    private Rigidbody snowball;
    private int currentSize;
    private float currentExp;

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
