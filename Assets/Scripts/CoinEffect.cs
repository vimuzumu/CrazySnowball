using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinEffect : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI amount;

    public IEnumerator AnimateCoinEffect(int coinsAmount)
    {
        float t = 0f;
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 from = rectTransform.anchoredPosition;
        Vector2 to = from + Vector2.up * 50f;
        amount.text = "+" + coinsAmount + " X ";
        while (t < 1f)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(from, to, t);
            t += Time.deltaTime / 0.4f;
            yield return null;
        }
        Destroy(gameObject);
    }
}
