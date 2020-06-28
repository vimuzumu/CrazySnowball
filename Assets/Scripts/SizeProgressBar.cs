using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeProgressBar : MonoBehaviour
{
    private float maxHight;
    private float currentExp;
    private GameManager gameManager;
    private RectTransform fill;

    void Start()
    {
        gameManager = GameManager.GetGameManager();
        maxHight = transform.parent.GetComponent<RectTransform>().rect.height;
        currentExp = gameManager.GetCurrentExp();
        fill = GetComponent<RectTransform>();
    }

    void FixedUpdate()
    {
        if (gameManager.GetCurrentExp() != currentExp)
        {
            currentExp = gameManager.GetCurrentExp();
            fill.sizeDelta = new Vector2(fill.sizeDelta.x, maxHight * Mathf.Clamp(currentExp / gameManager.GetExpForNextSize(), 0f, 1f));
        }
    }
}
