using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballController : MonoBehaviour
{
    private const float MOVE_STEP = 0.2f;

    private new Rigidbody rigidbody;
    private float horizontalVelocity;
    private Vector2 touchInitialPosition;
    private RaycastHit hit;
    private int eatingAmount;
    private GameManager gameManager;
    private float[] levelEndTapExp;

    void Start()
    {
        eatingAmount = 0;
        horizontalVelocity = 0f;
        touchInitialPosition = Vector2.zero;
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(Vector3.forward * Settings.baseMoveVelocity + Physics.gravity * rigidbody.mass, ForceMode.Impulse);
        gameManager = GameManager.GetGameManager();
        transform.localScale = Vector3.one * Settings.startingSize;
        levelEndTapExp = Settings.GetLevelEndTapExp();
    }

    void Update()
    {
        if (gameManager.IsStartedLevelEnd() && !gameManager.IsFinishedLevelEnd())
        {
            if (DidClick())
            {
                gameManager.IncTapEffect();
                gameManager.GainExp(gameManager.GetExpForNextSize() * levelEndTapExp[0] * gameManager.GetCurrentTapEffect() * levelEndTapExp[1]);
            } else
            {
                gameManager.DecTapEffect();
            }
        }
        if (ShouldMove())
        {
            if (!gameManager.DidGameStart())
            {
                gameManager.StartGame();
            }
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
        if (gameManager.IsGameRunning())
        {
            if (IsPassedLevelEndLine())
            {
                Vector3 toPosition = gameManager.GetLevelEndPosition() + 180f * Vector3.forward + 63f * Vector3.down;
                rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, new Vector3(toPosition.x - rigidbody.transform.position.x, rigidbody.velocity.y, Settings.baseMoveVelocity * 1.25f), MOVE_STEP * 2);
            }
            else
            {
                if (rigidbody.velocity.z < Settings.baseMoveVelocity)
                {
                    rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, Mathf.Lerp(rigidbody.velocity.z, Settings.baseMoveVelocity, MOVE_STEP * 0.1f));
                }
                if (Physics.Raycast(transform.position, Vector3.down, out hit, transform.localScale.y, 1 << LayerMask.NameToLayer("Ground")))
                {
                    gameManager.GainExp(Time.fixedDeltaTime * Settings.expGainPerSecond);
                }
                rigidbody.velocity = new Vector3(Mathf.Lerp(rigidbody.velocity.x, horizontalVelocity, MOVE_STEP), rigidbody.velocity.y, rigidbody.velocity.z);
            }
        }
        else if (rigidbody.velocity.magnitude != 0f)
        {
            rigidbody.velocity = Vector3.zero;
        }
    }

    private bool DidClick()
    {
        bool didClick = false;
#if UNITY_EDITOR
        return Input.GetMouseButtonDown(0);
#else
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                return touch.phase == TouchPhase.Began;
            }
#endif
        return didClick;
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
        float maxDistance = Screen.width * Settings.swipeScreenRatio;
        float distance = Mathf.Clamp(position.x - touchInitialPosition.x, -maxDistance, maxDistance);
        return (distance / maxDistance) * Settings.maxHorizontalVelocity;
    }
}
