using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //Variables (Unaccessable in inspector, due to lazy instantiation)
    public List<ItemBase> InventoryItems = new List<ItemBase>();

    private static Inventory instance;

    //Methods
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public static Inventory Instance
    {
        get { return instance ?? (instance = new GameObject("Inventory").AddComponent<Inventory>()); }
    }

    // Working!
    public void TesterMetod()
    {
        Debug.Log("I'm doing awesome stuff");
    }
}
