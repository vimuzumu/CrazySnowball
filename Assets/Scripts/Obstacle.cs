using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public const float SECONDS_TO_GET_EATEN = 2f;
    private const float EATEN_SCALE_DIFF_THRESHOLD = 0.75f;
    private const float OUTLINE_SCALE_DIFF_THRESHOLD = 3f;
    public const float SCALE_CHANGE_AMOUNT = 0.3f;

    private Outline outline;
    private Rigidbody snowball;
    private new Collider collider;

    void Start()
    {
        collider = GetComponent<Collider>();
        collider.isTrigger = false;
        snowball = GameManager.GetGameManager().GetSnowball();
        outline = GetComponent<Outline>();
        outline.enabled = false;
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
        return snowball.transform.localScale.y - transform.localScale.y > EATEN_SCALE_DIFF_THRESHOLD;
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
                other.GetComponent<SnowballController>().ChangeScale(-SCALE_CHANGE_AMOUNT);
                //if (ShouldShowOutline())
                //{
                //    other.GetComponent<SnowballController>().ChangeScale(-SCALE_CHANGE_AMOUNT);
                //    Destroy(gameObject);
                //    other.attachedRigidbody.velocity = Vector3.back * 15f;
                //}
                //else
                //{
                //    Vector3 newVelocity = (other.transform.position - transform.position).normalized * 50f;
                //    newVelocity.y = other.attachedRigidbody.velocity.y;
                //    other.attachedRigidbody.velocity = newVelocity;
                //}
            }
        }
    }

    public IEnumerator GetEaten(Transform snowball)
    {
        collider.enabled = false;
        transform.SetParent(snowball, true);
        StartCoroutine(snowball.GetComponent<SnowballController>().EatObstacle());
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
