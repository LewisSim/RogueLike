using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAllPlayers : MonoBehaviour
{
    private void Start()
    {
        var chars = GameObject.FindGameObjectsWithTag("Player");
        foreach (var item in chars)
        {
            Destroy(item);
        }
    }
}
