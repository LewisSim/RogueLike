using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHandler : MonoBehaviour
{
    private bool m_IsTriggered = false;
    public bool isTriggered { get { return m_IsTriggered; } }
    private Collider m_TriggeredBy = null;
    public Collider triggeredBy { get { return m_TriggeredBy; } }

    private void OnTriggerEnter(Collider other)
    {
        m_IsTriggered = true;
        m_TriggeredBy = other;
    }
    private void OnTriggerExit(Collider other)
    {
        m_IsTriggered = false;
        m_TriggeredBy = null;
    }
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("ontriggerstay" + m_TriggeredBy);
        if (m_TriggeredBy == null)
        {
            m_TriggeredBy = other;
            m_IsTriggered = true;
            //Debug.Log("added");
        }
    }

}
