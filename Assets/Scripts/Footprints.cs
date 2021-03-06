﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footprints : MonoBehaviour
{
    [Tooltip("this is how long the decal will stay, before it shrinks away totally")]
    public float Lifetime = 2.0f;

    private float mark;
    private Vector3 OrigSize;

    public void Start()
    {
        mark = Time.time;
        OrigSize = this.transform.localScale;
    }

    public void Update() // 100% time = 100% size,,, time goes down(%),, and so does the size until it's destroyed
    {
        float ElapsedTime = Time.time - mark;
        //Debug.Log(ElapsedTime);
        if (ElapsedTime != 0)
        {
            float PercentTimeLeft = (Lifetime - ElapsedTime) / Lifetime;
            //Debug.Log(PercentTimeLeft);

            this.transform.localScale = new Vector3(OrigSize.x * PercentTimeLeft, OrigSize.y * PercentTimeLeft, OrigSize.z * PercentTimeLeft);
            if (ElapsedTime > Lifetime)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
