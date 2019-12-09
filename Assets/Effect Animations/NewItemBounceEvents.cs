using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewItemBounceEvents : MonoBehaviour
{
    public void OnAnimationEnd()
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }
}
