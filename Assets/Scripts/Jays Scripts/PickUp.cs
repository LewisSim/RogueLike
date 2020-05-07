using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    //Variables
    ItemBase pickUp = new ItemBase();

    //Methods
    private void Start()
    {
        pickUp.itemName = "Tester";
        pickUp.itemDescription = "We're really testing here";
    }

    //void OnCollisionEnter(Collision collision)
    //{

    //    if (collision.gameObject.tag == "Player" && Inventory.Instance.InventoryItems.Count <= 4)
    //    {
    //        Inventory.Instance.InventoryItems.Add(pickUp);
    //        Destroy(gameObject);
    //        print(Inventory.Instance.InventoryItems[0].itemName);
    //    }
    //}

}
