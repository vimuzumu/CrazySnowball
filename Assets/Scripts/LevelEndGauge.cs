using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndGauge : MonoBehaviour
{
    private float maxHeight;
    private float currentValue;
    private GameManager gameManager;
    private RectTransform fill;

    void Start()
    {
        gameManager = GameManager.GetGameManager();
        maxHeight = transform.parent.GetComponent<RectTransform>().rect.height;
        currentValue = gameManager.GetCurrentExp();
        fill = GetComponent<RectTransform>();
    }

    void FixedUpdate()
    {
        if (gameManager.GetCurrentTapEffect() != currentValue)
        {
            currentValue = gameManager.GetCurrentTapEffect();
            fill.sizeDelta = new Vector2(fill.sizeDelta.x, maxHeight * Mathf.Clamp(currentValue / 100f, 0f, 1f));
        }
    }
}
