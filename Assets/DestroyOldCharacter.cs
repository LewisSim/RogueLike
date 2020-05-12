using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOldCharacter : MonoBehaviour
{
    private void Start()
    {
        var chars = GameObject.FindGameObjectsWithTag("Player");
        foreach (var item in chars)
        {
            if(item.gameObject != gameObject)
            {
                Destroy(item);
            }
        }
    }
}
