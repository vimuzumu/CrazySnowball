using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballController : MonoBehaviour
{
    private const float BASE_MOVE_SPEED = 50f;
    private const float MAX_HORIZONTAL_VELOCITY = 20f;
    private const float MOVE_STEP = 0.2f;
    private const float EXP_GAIN_PER_SECOND = 400f;

    private new Rigidbody rigidbody;
    private float horizontalVelocity;
    private Vector2 touchInitialPosition;
    private RaycastHit hit;
    private int eatingAmount;
    private GameManager gameManager;

    void Start()
    {
        eatingAmount = 0;
        horizontalVelocity = 0f;
        touchInitialPosition = Vector2.zero;
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(Vector3.forward * BASE_MOVE_SPEED + Physics.gravity * rigidbody.mass, ForceMode.Impulse);
        gameManager = GameManager.GetGameManager();
    }

    void Update()
    {
        if (gameManager.IsStartedLevelEnd() && !gameManager.IsFinishedLevelEnd())
        {
            if (Input.GetMouseButtonDown(0))
            {
                gameManager.IncTapEffect();
            } else
            {
                gameManager.DecTapEffect();
            }
        }
        if (ShouldMove())
        {
            horizontalVelocity = GetHorizontalVelocity();
        }
        else
        {
            horizontalVelocity = 0f;
        }
    }

    private bool IsPassedLevelEndLine()
    {
        if (gameManager.IsStartedLevelEnd() || rigidbody.position.z > gameManager.GetLevelEndPosition().z)
        {
            gameManager.StartLevelEnd();
            return true;
        }
        else
        {
            return false;
        }
    }

    private void FixedUpdate()
    {
        if (IsPassedLevelEndLine())
        {
            Vector3 toPosition = gameManager.GetLevelEndPosition() + 180f * Vector3.forward + 63f * Vector3.down;
            rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, new Vector3(toPosition.x - rigidbody.transform.position.x, rigidbody.velocity.y, BASE_MOVE_SPEED * 1.25f), MOVE_STEP * 2);
        }
        else
        {
            if (rigidbody.velocity.z < BASE_MOVE_SPEED)
            {
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, Mathf.Lerp(rigidbody.velocity.z, BASE_MOVE_SPEED, MOVE_STEP * 0.1f));
            }
            if (Physics.Raycast(transform.position, Vector3.down, out hit, transform.localScale.y, 1 << LayerMask.NameToLayer("Ground")))
            {
                gameManager.GainExp(Time.fixedDeltaTime * EXP_GAIN_PER_SECOND);
            }
            rigidbody.velocity = new Vector3(Mathf.Lerp(rigidbody.velocity.x, horizontalVelocity, MOVE_STEP), rigidbody.velocity.y, rigidbody.velocity.z);
        }
    }

    private bool ShouldMove()
    {
        bool shouldMove = false;
        #if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                touchInitialPosition = Input.mousePosition;
            }
            shouldMove = Input.GetMouseButton(0);
        #else
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    touchInitialPosition = touch.position;
                }
                shouldMove = true;
            }
        #endif
        return shouldMove;
    }

    private float GetHorizontalVelocity()
    {
        Vector2 position = Vector2.zero;
        #if UNITY_EDITOR
            position = Input.mousePosition;
        #else
		    foreach(Touch touch in Input.touches) 
            {
			    position = touch.position;
                break;
		    }
        #endif
        float maxDistance = Screen.width * 0.25f;
        float distance = Mathf.Clamp(position.x - touchInitialPosition.x, -maxDistance, maxDistance);
        return (distance / maxDistance) * MAX_HORIZONTAL_VELOCITY;
    }
}
