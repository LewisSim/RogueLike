using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAIToStart : MonoBehaviour
{
    public Unit unit;
    public EnemyLS enemy;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "charactermesh")
        {
            unit.target = other.transform;
            //print("player detected at: "+other.gameObject.transform.position);
            unit.enabled = true;
            enemy.enabled = true;
        }
    }
}
