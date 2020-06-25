using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float BASE_UP_OFFSET = 12f;
    private const float BASE_BACK_OFFSET = 20f;
    private const float MOVE_STEP = 0.1f;

    private Rigidbody snowball;
    private float upOffset;
    private float backOffset;

    // Start is called before the first frame update
    void Start()
    {
        snowball = GameManager.GetGameManager().GetSnowball();
        transform.rotation = Quaternion.Euler(15f, 0f, 0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        upOffset = BASE_UP_OFFSET * snowball.transform.localScale.y * 0.8f/* + Mathf.Floor(snowball.transform.localScale.y)*/;
        backOffset = BASE_BACK_OFFSET * snowball.transform.localScale.z * 0.8f/* + Mathf.Floor(snowball.transform.localScale.z)*/;
        transform.position = Vector3.Lerp(transform.position, snowball.position + Vector3.up * upOffset + Vector3.back * backOffset, MOVE_STEP);
    }
}
