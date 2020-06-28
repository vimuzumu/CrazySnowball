using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private const float SECONDS_TO_GET_EATEN = 2f;
    private const float SECONDS_TO_GET_PULLED = 1f;
    private const float OUTLINE_SCALE_DIFF_THRESHOLD = 3f;
    private const float SCALE_CHANGE_AMOUNT = 0.3f;
    private const float EXP_LOSS_PER_HIT = 20f;

    private Outline outline;
    private Rigidbody snowball;
    private new Collider collider;
    private GameManager gameManager;

    [SerializeField]
    private bool isLevelEndBlock;
    [SerializeField]
    private int size;
    [SerializeField]
    private GameObject coinEffectPrefab;

    void Start()
    {
        collider = GetComponent<Collider>();
        collider.isTrigger = false;
        gameManager = GameManager.GetGameManager();
        snowball = gameManager.GetSnowball();
        outline = GetComponent<Outline>();
        outline.enabled = false;
        size = size == 0 ? (int)transform.localScale.y : size;
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
        return gameManager.GetCurrentSize() >= size;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ShouldGetEaten())
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Snowball"))
            {
                StartCoroutine(GetEaten(other.transform));
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
                gameManager.LoseExp(EXP_LOSS_PER_HIT);
            }
        }
    }

    public IEnumerator GetEaten(Transform snowball)
    {
        collider.enabled = false;
        transform.SetParent(snowball, true);
        gameManager.GainExp(size * 10f);
        if (coinEffectPrefab != null)
        {
            GameManager.currentCoinsAmount += size;
            GameObject coinEffect = Instantiate(coinEffectPrefab, Camera.main.WorldToScreenPoint(snowball.position + Vector3.up * snowball.transform.localScale.y * 0.75f), Quaternion.identity, gameManager.GetCanvas().transform);
            StartCoroutine(coinEffect.GetComponent<CoinEffect>().AnimateCoinEffect(size));
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
}
