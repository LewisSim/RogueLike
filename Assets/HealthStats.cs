using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Stat
{

    [SerializeField]
    private HealthBarScript bar;

    [SerializeField]
    private float maxVal;

    //[SerializeField]
    public float currentVal;

    public float CurrentVal
    {
        get
        {
            return currentVal;
        }

        set
        {
            this.currentVal = Mathf.Clamp(value, 0, MaxVal);
            bar.Value = currentVal;
        }
    }

    public float MaxVal
    {
        get
        {
            return maxVal;
        }

        set
        {
            this.maxVal = value;
            bar.MaxValue = maxVal;
        }
    }
    public void Initialize()
    {
        this.MaxVal = maxVal;
        this.CurrentVal = currentVal;
    }



    public bool InEffect
    {
        get
        {
            return bar.InEffect;
        }
        set
        {
            bar.InEffect = value;
        }
    }

    //if current value == 100 then pause/disable player and pop up the endgame panel.
    //public gameobject panel
}
