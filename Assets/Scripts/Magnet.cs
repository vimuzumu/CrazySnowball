﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet: MonoBehaviour
{

    private new SphereCollider collider;

    private void Start()
    {
        collider = GetComponent<SphereCollider>();
        Enable(false);
    }

    private void Update()
    {

    }

    public void Enable(bool enable = true)
    {
        collider.enabled = enable;
    }

}
