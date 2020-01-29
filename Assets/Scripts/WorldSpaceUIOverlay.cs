using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorldSpaceUIOverlay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TextMeshProUGUI>().isOverlay = true;
    }
}
