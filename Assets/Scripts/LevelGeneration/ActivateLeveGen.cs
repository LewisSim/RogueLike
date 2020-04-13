using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateLeveGen : MonoBehaviour
{
    private void Awake()
    {
        LevelGen lGen = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGen>();
        if (lGen.loadBossRoom)
        {
            lGen.LoadBossRoom();
        }
        else
        {
            StartCoroutine(lGen.Begin());
        }

    }
}
