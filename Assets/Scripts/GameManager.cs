using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager gameManager;
    private Rigidbody snowball;
    [SerializeField]
    private GameObject LevelEndBlock;

    private bool startedLevelEnd;
    private bool jumpedLevelEnd;
    private bool collidedLevelEnd;

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
}
