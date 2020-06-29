using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRunner : MonoBehaviour
{
    private Vector3 positionDiff;
    private Transform snowballTransform;

    void Start()
    {
        snowballTransform = GameManager.GetGameManager().GetSnowball().transform;
        positionDiff = transform.position - snowballTransform.position;
    }

    void FixedUpdate()
    {
        transform.position = snowballTransform.position + positionDiff;
    }
}
