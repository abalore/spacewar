﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private int lifeTime;

    // Start is called before the first frame update
    void Start()
    {
        lifeTime = 60;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime--;
        if (lifeTime <= 0)
            Destroy(gameObject);
    }
}
