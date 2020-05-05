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
            if (Input.GetButton("Fire1"))
            {
                PickUp();
            }
        }
    }
    void PickUp()
    {
        Debug.Log("You got picked up an item");
        var sAtSource = gameObject.GetComponent<SoundAtSource>();
        sAtSource.indexOverride = 1;
        sAtSource.TriggerSoundAtUI();
        Destroy(gameObject.transform.parent.gameObject);
    }
}
