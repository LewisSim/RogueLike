using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    float rotateSpeed = 100f;
    private void Update()
    {
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        transform.Rotate(0, horizontal, 0);
    }
}
