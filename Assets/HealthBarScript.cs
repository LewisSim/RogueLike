using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    [SerializeField]
    public Stat health;

    [SerializeField]
    private float fillAmount;

    [SerializeField]
    private float lerpSpeed;

    [SerializeField]
    private Image content;

    [SerializeField]
    private Color fullColor;

    [SerializeField]
    private Color lowColor;

    public bool InEffect = false;


    public float MaxValue { get; set; }

    public float Value
    {
        set
        {
            //print("changing bar");
            fillAmount = Map(value, 0, MaxValue, 0, 1);
        }
    }

    void Awake()
    {
        health.Initialize();
    }


    void Update()
    {
        {
            HandleBar();
        }

        //if (Input.GetKeyDown(KeyCode.Q)) // take away 10hp
        //{
        //    health.CurrentVal -= 10;
        //}

        //if (Input.GetKeyDown(KeyCode.R)) // add 10hp
        //{
        //    health.CurrentVal += 10;
        //}

        if (health.CurrentVal <= 0)
        {
            print("Character Dead");
        }
    }

    private void HandleBar()
    {
        if (fillAmount != content.fillAmount)
        {
            content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);
        }

        content.color = Color.Lerp(lowColor, fullColor, fillAmount);

    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}

