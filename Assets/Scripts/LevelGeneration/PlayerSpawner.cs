using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject player;
    public minimapscript miniMap;

    public void SpawnPlayer()
    {
        //Place player at its location
        var s_player = Instantiate(player);
        s_player.transform.position = transform.position;
        s_player.transform.rotation = transform.rotation;
        miniMap.player = s_player.transform;

        //Destroy itself
        Destroy(gameObject);
    }
}
