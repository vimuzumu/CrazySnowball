using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinsAmount : MonoBehaviour
{
    private TextMeshProUGUI text;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        text.text = " X " + GameManager.currentCoinsAmount;
    }
}
