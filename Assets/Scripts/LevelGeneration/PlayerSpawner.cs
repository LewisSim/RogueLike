using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject player;
    //public minimapscript miniMap;

    public void SpawnPlayer()
    {
        GameObject s_player;

        //If player exists, move them, if not create player
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            s_player = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            s_player = Instantiate(player);

        }

        //Place player at its location
        s_player.transform.position = transform.position;
        s_player.transform.rotation = transform.rotation;
        //miniMap.player = s_player.transform;


        //Destroy itself
        Destroy(gameObject);
    }
}
