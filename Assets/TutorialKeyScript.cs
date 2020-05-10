using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialKeyScript : MonoBehaviour
{
    public SpawnPoint[] newEnemySpawners;

    [SerializeField]
    GameObject door;

    private void Start()
    {
        foreach (var spawner in newEnemySpawners)
        {
            spawner.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            TriggerDoor();
            TriggerSpawns();
            GetComponent<SoundAtSource>().TriggerSoundAtUI();
        }
    }

    void TriggerSpawns()
    {
        foreach (var spawner in newEnemySpawners)
        {
            spawner.gameObject.SetActive(true);
            spawner.Spawn();
        }
    }

    void TriggerDoor()
    {
        door.transform.Rotate(new Vector3(0f, 90f, 0f));
        //door.SetActive(false);
    }
}
