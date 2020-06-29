using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float BASE_UP_OFFSET = 12f;
    private const float BASE_BACK_OFFSET = 20f;
    private const float MOVE_STEP = 0.1f;
    private const float ANGLE = 15f;

    private Rigidbody snowball;
    private GameManager gameManager;
    private float upOffset;
    private float backOffset;

    [SerializeField]
    private ParticleSystem speedEffect;

    void Start()
    {
        gameManager = GameManager.GetGameManager();
        snowball = gameManager.GetSnowball();
        transform.rotation = Quaternion.Euler(ANGLE, 0f, 0f);
    }

    void FixedUpdate()
    {
        upOffset = BASE_UP_OFFSET * snowball.transform.localScale.y * 0.5f/* + Mathf.Floor(snowball.transform.localScale.y)*/;
        backOffset = BASE_BACK_OFFSET * snowball.transform.localScale.z * 0.5f/* + Mathf.Floor(snowball.transform.localScale.z)*/;
        if (gameManager.IsFinishedLevelEnd())
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(ANGLE * 1.25f, 0f, 0f), MOVE_STEP * 0.25f);
            transform.position = Vector3.Slerp(transform.position, snowball.position + Vector3.up * upOffset * 0.7f + Vector3.back * backOffset * 0.5f, MOVE_STEP * 0.75f);
        }
        else if (gameManager.IsStartedLevelEnd())
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(ANGLE * 2f, 0f, 0f), MOVE_STEP * 0.25f);
            transform.position = Vector3.Slerp(transform.position, snowball.position + Vector3.up * upOffset * 0.3f + Vector3.back * backOffset * 0.05f, MOVE_STEP * 0.75f);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, snowball.position + Vector3.up * upOffset + Vector3.back * backOffset, MOVE_STEP);
        }
    }

    public void PlaySpeedEffect(float emissionRate)
    {
        ParticleSystem.EmissionModule emission = speedEffect.emission;
        emission.rateOverTime = emissionRate;
        speedEffect.Play();
    }
}
