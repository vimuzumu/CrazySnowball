using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private const float SECONDS_TO_SANTA_HOP = 5f;
    private const float SECONDS_TO_GET_EATEN = 2f;
    private const float SECONDS_TO_GET_PULLED = 1f;
    //private const float OUTLINE_SCALE_DIFF_THRESHOLD = 3f; might be used to remove highlight for small objects

    private Outline outline;
    private Rigidbody snowball;
    private new Collider collider;
    private GameManager gameManager;
    private Animator animator;
    private bool snowman;

    [SerializeField]
    private bool isSanta;
    [SerializeField]
    private bool isLevelEndBlock;
    [SerializeField]
    private int size;
    [SerializeField]
    private GameObject collectibleEffectPrefab;

    void Start()
    {
        collider = GetComponent<Collider>();
        collider.isTrigger = false;
        gameManager = GameManager.GetGameManager();
        snowball = gameManager.GetSnowball();
        outline = GetComponent<Outline>();
        outline.enabled = false;
        size = size == 0 ? (int)transform.localScale.y : size;
        snowman = gameObject.layer == LayerMask.NameToLayer("Snowman");
        animator = GetComponent<Animator>();
        EnableAnimation(false);
    }

    void Update()
    {
        ColorOutline();
        if (isSanta)
        {
            SetScaleAsSnowball();
        }
    }

    private void EnableAnimation(bool enable = true)
    {
        if (animator)
        {
            animator.enabled = enable;
        }
    }

    private void ColorOutline()
    {
        if (ShouldGetEaten())
        {
            collider.isTrigger = true;
            EnableAnimation(true);
            if (!outline.enabled)
            {
                outline.enabled = true;
            }
            outline.OutlineColor = ShouldGetEaten() ? Color.green : Color.red;
        }
    }
    private bool ShouldGetEaten()
    {
        return snowman || gameManager.GetCurrentSize() >= size;
    }

    public void SetScaleAsSnowball()
    {
        transform.localScale = snowball.transform.localScale * 0.5f;
    }

    public void SetSize(int i)
    {
        size = i;
    }

    public void SetIsLevelEnd(bool b)
    {
        isLevelEndBlock = b;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ShouldGetEaten())
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Snowball"))
            {
                if (isSanta)
                {
                    StartCoroutine(SantaHop(other.transform));
                }
                else
                {
                    StartCoroutine(GetEaten(other.transform));
                }
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Magnet"))
            {
                StartCoroutine(GetPulled(other.transform));
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Snowball") && !ShouldGetEaten())
        {
            if (isLevelEndBlock)
            {
                gameManager.FinishLevel();
            }
            else
            {
                gameManager.LoseExp(Settings.expLossPerHit);
            }
        }
    }

    public IEnumerator GetEaten(Transform snowball)
    {
        collider.enabled = false;
        transform.SetParent(snowball, true);
        if (collectibleEffectPrefab != null)
        {
            size = isLevelEndBlock ? size * 10 : size;
            GameManager.currentCoinsAmount += size;
            GameObject collectibleEffect = Instantiate(collectibleEffectPrefab, Camera.main.WorldToScreenPoint(snowball.position + Vector3.up * snowball.transform.localScale.y * 0.75f), Quaternion.identity, gameManager.GetCanvas().transform);
            StartCoroutine(collectibleEffect.GetComponent<CollectibleEffect>().AnimateCollectibleEffect(size));
        }
        float t = 0f;
        Vector3 initialScale = transform.localScale;
        Vector3 initialPosition = transform.localPosition;
        while (t < 1f)
        {
            transform.localScale = Vector3.Lerp(initialScale, Vector3.one * 0.4f, t);
            transform.localPosition = Vector3.Lerp(initialPosition, Vector3.zero, t);
            t += Time.deltaTime / SECONDS_TO_GET_EATEN;
            yield return null;
        }
        if (!isLevelEndBlock)
        {
            gameManager.GainExp(snowman ? gameManager.GetExpForNextSize() * 0.5f : size * Settings.eatingExpMultiplier, isLevelEndBlock);
        }
        Destroy(gameObject);        
    }

    private IEnumerator GetPulled(Transform snowball)
    {
        collider.enabled = false;
        float t = 0f;
        Vector3 initialPosition = transform.position;
        while (t < 1f)
        {
            transform.position = Vector3.Lerp(initialPosition, snowball.position, t);
            t += Time.deltaTime / SECONDS_TO_GET_PULLED;
            yield return null;
        }
        StartCoroutine(GetEaten(snowball));
    }

    private IEnumerator SantaHop(Transform snowball)
    {
        collider.enabled = false;
        float t = 0f;
        Quaternion initialRotation = transform.rotation;
        while (t < 1f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, snowball.position + Vector3.up * snowball.transform.localScale.y, t);
            transform.rotation = Quaternion.Lerp(initialRotation, Quaternion.Euler(Vector3.zero), t);
            t += Time.deltaTime / (SECONDS_TO_SANTA_HOP * 0.1f);
            yield return null;
        }
        transform.localPosition = snowball.position + Vector3.up * snowball.transform.localScale.y;
        t = 0;
        while (t < 1f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, snowball.position + Vector3.up * snowball.transform.localScale.y * 0.5f, t);
            t += Time.deltaTime / (SECONDS_TO_SANTA_HOP * 0.1f);
            yield return null;
        }
        transform.localPosition = snowball.position + Vector3.up * snowball.transform.localScale.y * 0.5f;
        gameManager.Fever();
        float timeLeft = (SECONDS_TO_SANTA_HOP * 0.8f);
        while (timeLeft > 0f)
        {
            transform.localPosition = snowball.position + Vector3.up * snowball.transform.localScale.y * 0.5f;
            timeLeft -= Time.deltaTime;
            yield return null;
        }
        gameManager.FeverDone();
        StartCoroutine(GetEaten(snowball));
    }
}
