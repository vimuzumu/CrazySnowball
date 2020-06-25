using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private const float SECONDS_TO_GET_EATEN = 2f;
    private const float OUTLINE_SCALE_DIFF_THRESHOLD = 3f;
    private const float SCALE_CHANGE_AMOUNT = 0.3f;
    private const float EXP_LOSS_PER_HIT = 20f;

    private Outline outline;
    private Rigidbody snowball;
    private new Collider collider;
    private GameManager gameManager;

    [SerializeField]
    private int size;

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
        if (other.gameObject.layer == LayerMask.NameToLayer("Snowball"))
        {
            if (ShouldGetEaten())
            {
                StartCoroutine(GetEaten(other.transform));
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
}
