using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingArea : MonoBehaviour
{
    public GameObject spawnArea;
    public GameObject[] characterSpawns;

    private void Awake()
    {

    }
    public void PlaceObject(GameObject objectToPlace)
    {
        Vector3 spawnTransform = spawnArea.transform.position;

        objectToPlace.transform.position = spawnTransform;
    }
    public void RandomiseObject(GameObject objectToPlace)
    {
        int randomNum = Random.Range(1, 5);
        Vector3 spawnTransform = Vector3.zero;

        switch (randomNum)
        {
            case 1:
                spawnTransform = characterSpawns[0].transform.position;
                break;
            case 2:
                spawnTransform = characterSpawns[1].transform.position;
                break;
            case 3:
                spawnTransform = characterSpawns[2].transform.position;
                break;

            case 4:
                spawnTransform = characterSpawns[3].transform.position;
                break;

            case 5:
                spawnTransform = characterSpawns[4].transform.position;
                break;
        }

        objectToPlace.transform.position = spawnTransform;
        objectToPlace.transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));
    }
}
