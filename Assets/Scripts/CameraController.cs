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
        upOffset = BASE_UP_OFFSET + gameManager.GetCurrentSize();
        backOffset = BASE_BACK_OFFSET + gameManager.GetCurrentSize() * 1.1f;
        if (gameManager.IsStartedLevelEnd())
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(ANGLE * 0.75f, 0f, 0f), MOVE_STEP * 0.25f);
            transform.position = Vector3.Slerp(transform.position, snowball.position + Vector3.up * upOffset * 0.2f + Vector3.back * backOffset * 0.25f, LEVEL_END_MOVE_STEP);
        }
        else if (gameManager.IsFinishedLevelEnd())
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(ANGLE * 0.5f, 0f, 0f), MOVE_STEP * 0.25f);
            transform.position = Vector3.Slerp(transform.position, snowball.position + Vector3.up * upOffset * 0.4f + Vector3.back * backOffset * 0.25f, LEVEL_END_MOVE_STEP);
        }
        else
        {
            transform.rotation = Quaternion.Euler(ANGLE + snowball.transform.localScale.y * 0.15f, 0f, 0f);
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
