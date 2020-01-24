using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public void CameraFace(GameObject obj, Camera camera)
    {
        obj.transform.rotation = Quaternion.LookRotation(transform.position - camera.transform.position);
    }
}
