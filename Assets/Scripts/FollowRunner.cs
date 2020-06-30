using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRunner : MonoBehaviour
{
    private Vector3 positionDiff;
    private Transform snowballTransform;
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.GetGameManager();
        snowballTransform = gameManager.GetSnowball().transform;
        positionDiff = transform.position - snowballTransform.position;
    }

    void FixedUpdate()
    {
        transform.position = snowballTransform.position + positionDiff;
        transform.position += Vector3.up * gameManager.GetCurrentSize() * 0.5f;
    }
}
