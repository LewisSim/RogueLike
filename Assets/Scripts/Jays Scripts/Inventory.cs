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

    public Sprite[] icons_inspector;
    static Sprite[] icons;

    public static bool isFull;
    static bool[] slotsFilled; 

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        p_inventory = new Item[4];
        for (int i = 0; i < p_inventory.Length; i++)
        {
            p_inventory[i] = null;
        }

        icons = icons_inspector;
        ui_slots = ui_slots_inspector;
        slotsFilled = new bool[p_inventory.Length];
    }

    public static void AddItem(Item item)
    {
        //Find empty slot to fill
        for (int i = 0; i < p_inventory.Length; i++)
        {
            if(p_inventory[i] == null)
            {
                p_inventory[i] = item;
                break;
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

        //Remove from array
        RemoveItem(index);

        //Update ui
        UpdateUI();
    }

    static void UpdateUI()
    {
        for (int i = 0; i < p_inventory.Length; i++)
        {
            if(p_inventory[i] != null)
            {
                int iconType = 0;

                switch (p_inventory[i].Type)
                {
                    case Item.ItemType.Armour:
                        iconType = 0;
                        break;
                    case Item.ItemType.Potion:
                        iconType = 1;
                        break;
                    case Item.ItemType.Weapon:
                        iconType = 2;
                        break;
                }

                var ui_icon = ui_slots[i].GetComponentsInChildren<SVGImage>()[1];

                ui_icon.name = p_inventory[i].GetFullName();
                ui_icon.sprite = icons[iconType];
                ui_icon.color = new Color(255, 255, 255, 255);

                slotsFilled[i] = true;
            }
            if (p_inventory[i] == null)
            {
                var ui_icon = ui_slots[i].GetComponentsInChildren<SVGImage>()[1];
                ui_icon.sprite = null;
                ui_icon.color = new Color(255, 255, 255, 0);
                slotsFilled[i] = false;
            }
        }


        //Check if full

        int checker = 0;
        foreach (var slot in slotsFilled)
        {
            if(slot == false)
            {
                isFull = false;
            }
            else
            {
                checker++;
            }
        }
        if(checker == slotsFilled.Length)
        {
            isFull = true;
        }

        Debug.Log("Inventory is full : " + isFull);
    }
}
