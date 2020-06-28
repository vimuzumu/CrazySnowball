using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet: MonoBehaviour
{

    private SphereCollider collider;

    private void Start()
    {
        collider = GetComponent<SphereCollider>();
        collider.enabled = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Enable();
        }
    }

    public void Enable()
    {
        collider.enabled = true;
        StartCoroutine(EnableFor2Seconds());
    }

    private IEnumerator EnableFor2Seconds()
    {
        float countDown = 4f;
        while (countDown >= 0)
        {
            countDown -= Time.smoothDeltaTime;
            yield return null;
        }
        collider.enabled = false;
    }

}
