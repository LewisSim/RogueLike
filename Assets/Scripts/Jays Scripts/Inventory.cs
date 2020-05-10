using System.Collections;
using System.Collections.Generic;
using UnityEngine;


////Original Script
//public class Inventory : MonoBehaviour
//{

//    //Variables (Unaccessable in inspector, due to lazy instantiation)
//    public List<ItemBase> InventoryItems = new List<ItemBase>();

//    private static Inventory instance;

//    //Methods
//    private void Awake()
//    {
//        DontDestroyOnLoad(gameObject);
//    }
//    public static Inventory Instance
//    {
//        get { return instance ?? (instance = new GameObject("Inventory").AddComponent<Inventory>()); }
//    }

//    // Working!
//    public void TesterMetod()
//    {
//        Debug.Log("I'm doing awesome stuff");
//    }
//}


public class Inventory : MonoBehaviour
{
    public static Item[] p_inventory;

    public GameObject[] ui_slots_inspector;
    static GameObject[] ui_slots;

    public GameObject basicItem_inspector;
    static GameObject basicItem;

    public Color c_common_i, c_uncommon_i, c_rare_i, c_legendary_i, c_health_i, c_speed_i, c_damage_i;
    static Color c_common, c_uncommon, c_rare, c_legendary, c_health, c_speed, c_damage;

    static Transform playerPosition;

    public Sprite[] icons_inspector;
    static Sprite[] icons;

    public static bool isFull;
    static bool[] slotsFilled;

    //private static Inventory instance;

    //public static Inventory Instance
    //{
    //    get { return instance; }
    //}

    private void Awake()
    {

        Init();
    }

    public void Init()
    {
        p_inventory = new Item[5];
        for (int i = 0; i < p_inventory.Length; i++)
        {
            p_inventory[i] = null;
        }

        icons = icons_inspector;
        ui_slots = ui_slots_inspector;
        basicItem = basicItem_inspector;
        playerPosition = transform;
        c_common = c_common_i;
        c_uncommon = c_uncommon_i;
        c_rare = c_rare_i;
        c_legendary = c_legendary_i;
        c_health = c_health_i;
        c_speed = c_speed_i;
        c_damage = c_damage_i;

        slotsFilled = new bool[p_inventory.Length];
    }

    public static void AddItem(Item item)
    {
        ////Find empty slot to fill
        //for (int i = 0; i < p_inventory.Length; i++)
        //{
        //    if(p_inventory[i] == null)
        //    {
        //        p_inventory[i] = item;
        //        break;
        //    }
        //}

        //Check appropriate slot is empty
        if(item.Type == Item.ItemType.Armour)
        {
            if(p_inventory[4] == null)
            {
                p_inventory[4] = item;
            }
            else
            {
                DropItem(4);
                p_inventory[4] = item;
            }
        }

        if (item.Type == Item.ItemType.Potion)
        {
            if (p_inventory[3] == null)
            {
                p_inventory[3] = item;
            }
            else
            {
                DropItem(3);
                p_inventory[3] = item;
            }
        }

        if (item.SubType == Item.ItemSubType.Sword)
        {
            if (p_inventory[0] == null)
            {
                p_inventory[0] = item;
            }
            else
            {
                DropItem(0);
                p_inventory[0] = item;
            }
        }

        if (item.SubType == Item.ItemSubType.Spear)
        {
            if (p_inventory[2] == null)
            {
                p_inventory[2] = item;
            }
            else
            {
                DropItem(2);
                p_inventory[2] = item;
            }
        }

        if (item.SubType == Item.ItemSubType.Bow)
        {
            if (p_inventory[1] == null)
            {
                p_inventory[1] = item;
            }
            else
            {
                DropItem(1);
                p_inventory[1] = item;
            }
        }

        UpdateUI();
    }

    public static void RemoveItem(int index)
    {
        p_inventory[index] = null;
    }

    public static void DropItem(int index)
    {
        //Code to drop
        var newItem = Instantiate(basicItem);

        newItem.transform.position = playerPosition.localPosition + (playerPosition.forward * 2);

        var newStats = newItem.GetComponent<ItemStats>();
        newStats.LoadStats(p_inventory[index]);

        //Remove from array
        RemoveItem(index);

        //Update ui
        UpdateUI();
    }

    public static void UpdateUI()
    {
        for (int i = 0; i < p_inventory.Length; i++)
        {
            if(p_inventory[i] != null)
            {
                int iconType = 0;

                switch (p_inventory[i].SubType)
                {
                    case Item.ItemSubType.LightArmour:
                        iconType = 0;
                        break;
                    case Item.ItemSubType.MediumArmour:
                        iconType = 1;
                        break;
                    case Item.ItemSubType.HeavyArmour:
                        iconType = 2;
                        break;
                    case Item.ItemSubType.HealthPot:
                        iconType = 3;
                        break;
                    case Item.ItemSubType.SpeedPot:
                        iconType = 4;
                        break;
                    case Item.ItemSubType.DamagePot:
                        iconType = 5;
                        break;
                    case Item.ItemSubType.Sword:
                        iconType = 6;
                        break;
                    case Item.ItemSubType.Spear:
                        iconType = 7;
                        break;
                    case Item.ItemSubType.Bow:
                        iconType = 8;
                        break;
                }

                var ui_icon = ui_slots[i].GetComponentsInChildren<SVGImage>()[1];

                ui_icon.name = p_inventory[i].GetFullName();
                ui_icon.sprite = icons[iconType];
                //Set colour based on type (pot) or tier
                if(p_inventory[i].Type == Item.ItemType.Potion)
                {
                    switch (p_inventory[i].SubType)
                    {
                        case Item.ItemSubType.HealthPot:
                            ui_icon.color = c_health;
                            break;
                        case Item.ItemSubType.DamagePot:
                            ui_icon.color = c_damage;
                            break;
                        case Item.ItemSubType.SpeedPot:
                            ui_icon.color = c_speed;
                            break;
                    }
                }
                else
                {
                    switch (p_inventory[i].Tier)
                    {
                        case Item.ItemTier.Common:
                            ui_icon.color = c_common;
                            break;
                        case Item.ItemTier.Uncommon:
                            ui_icon.color = c_uncommon;
                            break;
                        case Item.ItemTier.Rare:
                            ui_icon.color = c_rare;
                            break;
                    }
                }
                ui_icon.color = new Color(ui_icon.color.r, ui_icon.color.g, ui_icon.color.b, 255);

                slotsFilled[i] = true;
            }
            if (p_inventory[i] == null)
            {
                var ui_icon = ui_slots[i].GetComponentsInChildren<SVGImage>()[1];
                ui_icon.sprite = null;
                ui_icon.color = new Color(ui_icon.color.r, ui_icon.color.g, ui_icon.color.b, 0);
                slotsFilled[i] = false;
            }
        }


        //Check if full

        //int checker = 0;
        //foreach (var slot in slotsFilled)
        //{
        //    if(slot == false)
        //    {
        //        isFull = false;
        //    }
        //    else
        //    {
        //        checker++;
        //    }
        //}
        //if(checker == slotsFilled.Length)
        //{
        //    isFull = true;
        //}

        //Debug.Log("Inventory is full : " + isFull);
    }
}
