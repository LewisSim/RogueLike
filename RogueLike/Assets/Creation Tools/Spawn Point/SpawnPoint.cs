using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public enum SpawnerType { Enemy, LootChest, Trap};
    public SpawnerType type;
    public Item.ItemTier tier;
    public bool test;

    [SerializeField] GameObject s_enemy, s_lootChest, s_trap;

    private void Start()
    {
        if (test)
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        //Destroy marker
        Destroy(GetComponentInChildren<MeshFilter>().gameObject);

        GameObject newItem = NewItem();
        newItem.transform.rotation = transform.rotation;
    }

    private GameObject NewItem()
    {
        switch (type)
        {
            case SpawnerType.Enemy:
                var eSpawn = Instantiate(s_enemy, transform);
                return eSpawn;
            case SpawnerType.LootChest:
                var lSpawn = Instantiate(s_lootChest, transform);
                return lSpawn;
            case SpawnerType.Trap:
                var tSpawn = Instantiate(s_trap, transform);
                return tSpawn;
            default:
                return null;
        }
    }
}
