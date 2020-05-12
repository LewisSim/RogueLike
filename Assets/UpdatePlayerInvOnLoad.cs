using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePlayerInvOnLoad : MonoBehaviour
{
    public PlayerSpawner playerspawner;
    public bool inDungeonScene;

    private void Start()
    {
        if (!inDungeonScene)
        {
            playerspawner.SpawnPlayer();
        }
        Inventory.UpdateUI();
        print("UI updated on load");

    }
}
