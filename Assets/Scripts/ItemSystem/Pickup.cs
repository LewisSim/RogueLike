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
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (Inventory.isFull)
                {
                    //Trigger "cannot pickup" sound
                    var sAtSource = gameObject.GetComponent<SoundAtSource>();
                    sAtSource.indexOverride = 6;
                    sAtSource.TriggerSoundAtUI();
                    QuickMessage.Message("Inventory Full", 0.5f);
                }
                else
                {
                    PickUp();
                }
            }
        }
    }
    void PickUp()
    {
        Debug.Log("You got picked up an item");
        var sAtSource = gameObject.GetComponent<SoundAtSource>();
        sAtSource.indexOverride = 1;
        sAtSource.TriggerSoundAtUI();

        Inventory.AddItem(gameObject.GetComponent<ItemStats>().GetStats());

        Destroy(gameObject);
    }
}
