using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeTrap : MonoBehaviour
{
    public float maxRot, minRot, speed;
    public bool swinging;

    private Vector3 minRotation, maxRotation;
    //Direction: false = moving toward max, true = moving toward min
    public bool direction;

    private void Awake()
    {
        //minRotation = new Vector3(0f, 0f, minRot);
        //maxRotation = new Vector3(0f, 0f, maxRot);
        //transform.eulerAngles = minRotation;
    }
    public void TriggerToggleSwinging()
    {
        swinging = !swinging;
    }
    // Update is called once per frame
    void Update()
    {
        if (swinging)
        {
            if (!direction)
            {
                if (transform.localEulerAngles.z < maxRot)
                {
                    var newAngle = new Vector3(0f, 0f, Mathf.LerpAngle(transform.localEulerAngles.z, maxRot, Time.deltaTime * speed));
                    transform.localEulerAngles = newAngle;
                    //Debug.Log(transform.eulerAngles.z);
                }
                if(transform.localEulerAngles.z >= maxRot-1)
                {
                    direction = true;
                    //Debug.Log("flip");
                }
            }


            if (direction)
            {
                if (transform.localEulerAngles.z > minRot)
                {
                    var newAngle = new Vector3(0f, 0f, Mathf.LerpAngle(transform.localEulerAngles.z, minRot, Time.deltaTime * speed));
                    transform.localEulerAngles = newAngle;
                    //Debug.Log(transform.eulerAngles.z);
                }
                if(transform.localEulerAngles.z <= minRot+1)
                {
                    direction = false;
                    //Debug.Log("flip2");
                }
            }
        }
    }
}
