using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgressBar : MonoBehaviour
{
    private RectTransform fillRectTransform;
    private float maxWidth;
    private Transform snowball;
    private float startZ;
    private float finishZ;


    // Start is called before the first frame update
    void Start()
    {
        GameManager gameManager = GameManager.GetGameManager();
        snowball = gameManager.GetSnowball().transform;
        startZ = snowball.position.z;
        finishZ = gameManager.GetLevelEndPosition().z;
        fillRectTransform = GetComponent<RectTransform>();
        maxWidth = transform.parent.GetComponent<RectTransform>().rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        fillRectTransform.sizeDelta = new Vector2(Mathf.InverseLerp(startZ, finishZ, snowball.position.z) * maxWidth, fillRectTransform.sizeDelta.y);
    }
}
