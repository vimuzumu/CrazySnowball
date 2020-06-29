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
    }

    void Update()
    {
        ColorOutline();
    }

    private void ColorOutline()
    {
        if (ShouldGetEaten())
        {
            collider.isTrigger = true;
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
                Debug.Log("Level Finished");
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
        gameManager.GainExp(snowman ? gameManager.GetExpForNextSize() * 0.5f : size * Settings.eatingExpMultiplier, isLevelEndBlock);
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
        Vector3 initialPosition = transform.localPosition;
        while (t < 0.1f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, snowball.position + Vector3.up * snowball.transform.localScale.y * 2f, t * 5);
            t += Time.deltaTime / SECONDS_TO_SANTA_HOP;
            yield return null;
        }
        transform.localPosition = snowball.position + Vector3.up * snowball.transform.localScale.y * 2f;
        while (t < 0.2f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, snowball.position + Vector3.up * snowball.transform.localScale.y * 0.5f, t * 5);
            t += Time.deltaTime / SECONDS_TO_SANTA_HOP;
            yield return null;
        }
        transform.localPosition = snowball.position + Vector3.up * snowball.transform.localScale.y * 0.5f;
        gameManager.Fever();
        while (t < 1f) 
        {
            transform.localPosition = snowball.position + Vector3.up * snowball.transform.localScale.y * 0.5f;
            t += Time.deltaTime / SECONDS_TO_SANTA_HOP;
            yield return null;
        }
        gameManager.FeverDone();
        StartCoroutine(GetEaten(snowball));
        //Destroy(gameObject);
    }
}
