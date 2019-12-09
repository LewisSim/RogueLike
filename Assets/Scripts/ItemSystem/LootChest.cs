using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootChest : MonoBehaviour
{
    Drop dropScript;
    // Start is called before the first frame update
    void Start()
    {
        dropScript = GetComponent<Drop>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Open();
        }
    }

    void Open()
    {
        dropScript.DropItem();
        Destroy(gameObject);
    }
}
