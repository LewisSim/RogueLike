using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public TriggerHandler detectCollider;

    // Update is called once per frame
    void Update()
    {
        if (detectCollider.isTriggered && detectCollider.triggeredBy.gameObject.tag == "Player")
        {
            if (Input.GetMouseButtonDown(0))
            {
                PickUp();
            }
        }
    }
    void PickUp()
    {
        Debug.Log("You got picked up an item");
        Destroy(gameObject.transform.parent.gameObject);
    }
}
