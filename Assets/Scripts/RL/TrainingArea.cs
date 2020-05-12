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
        objectToPlace.transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));
    }
}
