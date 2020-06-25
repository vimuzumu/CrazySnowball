using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeProgressBar : MonoBehaviour
{
    private float maxWidth;
    private float currentExp;
    private GameManager gameManager;
    private RectTransform fill;

    void Start()
    {
        gameManager = GameManager.GetGameManager();
        maxWidth = transform.parent.GetComponent<RectTransform>().rect.width;
        currentExp = gameManager.GetCurrentExp();
        fill = GetComponent<RectTransform>();
    }

    void FixedUpdate()
    {
        if (gameManager.GetCurrentExp() != currentExp)
        {
            currentExp = gameManager.GetCurrentExp();
            fill.sizeDelta = new Vector2(maxWidth * Mathf.Clamp(currentExp / gameManager.GetExpForNextSize(), 0f, 1f), fill.sizeDelta.y);
        }
    }
}
