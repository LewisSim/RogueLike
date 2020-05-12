using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingCharacter : MonoBehaviour
{
    bool m_OnBallista;
    bool m_InRange;
    public int health;
    float inputTime = 5f;
    float inputTop = 10f;
    float timer;
    GameObject cover1, cover2, cover3, cover4;

    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        timer = SetTimer();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer -= Time.deltaTime;

        if (timer < 0f)
        {
            if (m_OnBallista)
            {
                m_OnBallista = false;

            }
            else
            {
                m_OnBallista = true;
            }
            timer = SetTimer();
        }
    }

    public bool UsingBallista()
    {
        return m_OnBallista;
    }

    public bool AgentInRange()
    {
        return m_InRange;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("agent"))
        {
            m_InRange = true;

        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("agent"))
        {
            m_InRange = false;

        }
    }

    public void ResetValues()
    {
        m_InRange = false;
        m_OnBallista = false;
        health = 100;
        SetTimer();
    }


    float SetTimer()
    {
        float number = inputTime;
        number = Random.Range(inputTime, inputTop);

        return number;
    }
}
