using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectibleEffect : MonoBehaviour
{
    [SerializeField]
    private bool coinCollectible;
    [SerializeField]
    private TextMeshProUGUI amountText;

    public IEnumerator AnimateCollectibleEffect(int amount)
    {
        if (coinCollectible)
        {
            GameManager.currentCoinsAmount += amount;
        }
        float t = 0f;
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 from = rectTransform.anchoredPosition;
        Vector2 to = from + Vector2.up * 50f;
        amountText.text = "+" + amount;
        while (t < 1f)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(from, to, t);
            t += Time.deltaTime / 0.5f;
            yield return null;
        }
        Destroy(gameObject);
    }
}
