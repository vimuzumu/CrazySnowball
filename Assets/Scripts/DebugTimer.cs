using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugTimer : MonoBehaviour
{
    private TextMeshProUGUI text;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.GetGameManager();
        text = GetComponent<TextMeshProUGUI>();
        text.text = "0.00";
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GetGameManager().DidGameStart())
        {
            text.text = (Time.time - gameManager.GetGameStartTime()).ToString("F2");
        }
    }
}
