using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager gameManager;
    private Rigidbody snowball;

    private void Awake()
    {
        gameManager = this;
        snowball = GameObject.Find("Snowball").GetComponent<Rigidbody>();
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
}
