using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupnDown : MonoBehaviour
{

    //Camera variables
    public bool lockCursor;
    public float mouseSensitivity = 5;
    public Vector2 pitchMinMax = new Vector2(-40, 85);
    public float rotationSmoothTime = .12f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;
    Vector3 newpos;
    float pitch;

    void Start()
    {
        
    }

    void Update()
    {
        pitch -= Input.GetAxis("Mouse y") * mouseSensitivity;
        //pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
        //currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(0, 0, pitch), ref rotationSmoothVelocity, rotationSmoothTime);
        //transform.eulerAngles = currentRotation;

        //transform.position.x += mouseSensitivity * Input.GetAxis("Horizontal") * Time.deltaTime;
        //Vector3 newpos = transform.position;
        //newpos.y += mouseSensitivity * Input.GetAxis("Verticle") * Time.deltaTime;
        //transform.position = newpos;

        //Quaternion rotationLook = new Quaternion();
        // rotationLook.eulerAngles = new Vector3(0, pitch, 0);
        //rotationLook.eulerAngles = new Vector3(pitch, transform.position.y, transform.position.z);
        //transform.rotation = rotationLook;
        // transform.up = rotationLook.eulerAngles

        transform.Rotate(Vector3.right * mouseSensitivity * Time.deltaTime);
    }
}