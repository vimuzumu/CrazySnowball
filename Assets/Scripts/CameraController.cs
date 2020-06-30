using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float BASE_UP_OFFSET = 30f;
    private const float BASE_BACK_OFFSET = 25f;
    private const float MOVE_STEP = 0.1f;
    private const float ANGLE = 30f;
    private const float LEVEL_END_MOVE_STEP = 0.2f;

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
        int ballSize = gameManager.GetCurrentSize();
        upOffset = BASE_UP_OFFSET + ballSize;
        backOffset = BASE_BACK_OFFSET + ballSize * 1.1f;
        if (gameManager.IsStartedLevelEnd())
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(ANGLE * 0.75f, 0f, 0f), MOVE_STEP * 0.25f);
            transform.position = Vector3.Slerp(transform.position, snowball.position + Vector3.up * upOffset * 0.2f + Vector3.back * backOffset * 0.25f, LEVEL_END_MOVE_STEP);
        }
        else if (gameManager.IsFinishedLevelEnd())
        {
            if (gameManager.IsGameRunning())
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(ANGLE * 0.5f, 0f, 0f), MOVE_STEP * 0.25f);
                transform.position = Vector3.Slerp(transform.position, snowball.position + Vector3.up * upOffset * 0.4f + Vector3.back * backOffset * 0.25f, LEVEL_END_MOVE_STEP);
            } else
            {
                Vector3 originalForward = transform.forward;
                transform.LookAt(snowball.position);
                transform.forward = Vector3.Slerp(originalForward, transform.forward, MOVE_STEP * 0.25f);
                transform.position = Vector3.Slerp(transform.position, snowball.position + Vector3.up * upOffset * 0.8f + Vector3.back * backOffset * 0.3f, LEVEL_END_MOVE_STEP * 0.02f);
            }
        }
        else
        {
            transform.rotation = Quaternion.Euler(ANGLE + ballSize * 0.15f, 0f, 0f);
            if (ballSize < 5)
            {
                transform.position = Vector3.Lerp(transform.position, snowball.position + Vector3.up * upOffset * 0.5f + Vector3.back * backOffset * 0.5f, MOVE_STEP);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, snowball.position + Vector3.up * upOffset * 0.75f + Vector3.back * backOffset * 0.75f, MOVE_STEP);
            }
        }
    }

    public void PlaySpeedEffect(float emissionRate)
    {
        ParticleSystem.EmissionModule emission = speedEffect.emission;
        emission.rateOverTime = emissionRate;
        speedEffect.Play();
    }
}
