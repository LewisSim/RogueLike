using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCameraNoYOpposite : MonoBehaviour
{

    public GameObject player;

    private GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        obj = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        var lookAt = player.transform.position;
        lookAt.y = gameObject.transform.position.y;
        transform.LookAt(player.transform);
    }
}
