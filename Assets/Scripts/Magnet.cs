using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet: MonoBehaviour
{

    private new SphereCollider collider;

    private void Start()
    {
        collider = GetComponent<SphereCollider>();
        collider.radius = Settings.magnetRadius;
        collider.isTrigger = true;
        Enable(false);
    }

    public void Enable(bool enable = true)
    {
        collider.enabled = enable;
    }

}
