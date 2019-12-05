using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    public GameObject basicItemPrefab;
    public bool test = false;
    public int[] typeWeightTable =
    {
        45,
        30,
        25
    };

    public int[] tierWeightTable =
    {
        70,
        20,
        10
    };
    public void DropItem()
    {
        if (test)
        {
            var items = GameObject.FindGameObjectsWithTag("Item");
            if(items != null)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    Destroy(items[i]);
                }
            }
        }
        GameObject newItem = Instantiate(basicItemPrefab);
        newItem.transform.position = transform.position;

        int typeWeightTotal = 0, tierWeightTotal = 0;
        for (int i = 0; i < typeWeightTable.Length; i++)
        {
            typeWeightTotal += typeWeightTable[i];
        }
        for (int i = 0; i < tierWeightTable.Length; i++)
        {
            tierWeightTotal += tierWeightTable[i];
        }

        //Set type based on weights
        var newItemStats = newItem.GetComponent<ItemStats>();
        int rng = Random.Range(0, typeWeightTotal);
        Debug.Log("random number for type: " + rng);
        for (int i = 0; i < typeWeightTable.Length; i++)
        {
            if(rng <= typeWeightTable[i])
            {
                newItemStats.type = (Item.ItemType)i;
                break;
            }
            else
            {
                rng -= typeWeightTable[i];
            }
        }


        //Set tier based on weights
        rng = Random.Range(0, tierWeightTotal);
        Debug.Log("random number for tier: " + rng);
        for (int i = 0; i < tierWeightTable.Length; i++)
        {
            if(rng <= tierWeightTable[i])
            {
                newItemStats.tier = (Item.ItemTier)i;
                break;
            }
            else
            {
                rng -= tierWeightTable[i];
            }
        }


        newItemStats.AssignStats();
        //var vals =  newItemStats.GetStats();
        //Debug.Log("Type: " + vals.Type);
        //Debug.Log("Tier: " + vals.Tier);
        //Debug.Log("base name: " + vals.BaseName);
        //Debug.Log("prefix: " + vals.Prefix);
        //Debug.Log("suffix: " + vals.Suffix);
        //Debug.Log("Armour: " + vals.ArmourRating);
        //Debug.Log("Damage: " + vals.Damage);
        //Debug.Log("Health: " + vals.Health);
        //Debug.Log("Speed: " + vals.Speed);
        //Debug.Log("Potency: " + vals.Potency);
    }
}
