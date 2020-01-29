using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCameraNoY : MonoBehaviour
{
    
    public Camera camera;

    private GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        obj = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        var lookAt = camera.transform.position;
        lookAt.y = gameObject.transform.position.y;
        obj.transform.rotation = Quaternion.LookRotation(transform.position - lookAt);
    }
}
