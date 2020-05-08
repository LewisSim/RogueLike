using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ItemDesigner : MonoBehaviour
{
    Item.ItemType type;
    float armourRating, damage, health, speed;
    string baseName, prefix, suffix, fullName;
    public Item.ItemTier tier;
    public Item.ItemSubType subType;

    public float potency;

    Item itemOut;

    private void Awake()
    {
        Gen();
    }

    void ExportItem()
    {
        itemOut.Tier = tier;
        itemOut.Type = type;
        itemOut.ArmourRating = armourRating;
        itemOut.Health = health;
        itemOut.Damage = damage;
        itemOut.Speed = speed;
        itemOut.Potency = potency;
        itemOut.BaseName = baseName;
        itemOut.Prefix = prefix;
        itemOut.Suffix = suffix;
        itemOut.SubType = subType;


        GetComponent<ItemStats>().LoadStats(itemOut);
    }

    void Gen()
    {
        switch (subType)
        {
            case Item.ItemSubType.HealthPot:
                type = Item.ItemType.Potion;
                SetPotion();
                break;
            case Item.ItemSubType.DamagePot:
                type = Item.ItemType.Potion;
                SetPotion();
                break;
            case Item.ItemSubType.SpeedPot:
                type = Item.ItemType.Potion;
                SetPotion();
                break;
            case Item.ItemSubType.Sword:
                type = Item.ItemType.Weapon;
                SetWeapon();
                break;
            case Item.ItemSubType.Spear:
                type = Item.ItemType.Weapon;
                SetWeapon();
                break;
            case Item.ItemSubType.Bow:
                type = Item.ItemType.Weapon;
                SetWeapon();
                break;
        }

        ExportItem();
    }

    void SetPotion()
    {
        switch (tier)
        {
            case Item.ItemTier.Common:
                potency = Potion.Potency[0];
                break;
            case Item.ItemTier.Uncommon:
                potency = Potion.Potency[1];
                break;
            case Item.ItemTier.Rare:
                potency = Potion.Potency[2];
                break;
        }
    }

    void SetWeapon()
    {
        //switch (subType)
        //{
        //    case Item.ItemSubType.Sword:
                
        //}

    }
}


//[CustomEditor(typeof(ItemDesigner))]
//public class ItemDesignerEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        var itemDesigner = target as ItemDesigner;

//        itemDesigner
//    }
//}
