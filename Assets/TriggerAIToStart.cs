using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAIToStart : MonoBehaviour
{
    public Unit unit;
    public EnemyLS enemy;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            unit.target = other.transform;
            unit.enabled = true;
            enemy.enabled = true;
        }
    }
}
