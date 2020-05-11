using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Ballista : MonoBehaviour
{
    public GameObject player;

    public GameObject txtCanvas;
    public TextMeshProUGUI overlayText;

    bool playerIsInBounds = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            txtCanvas.SetActive(true);
            overlayText.GetComponent<TextMeshProUGUI>().isOverlay = true;
            playerIsInBounds = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            txtCanvas.SetActive(false);
            overlayText.GetComponent<TextMeshProUGUI>().isOverlay = false;
            playerIsInBounds = false;
        }
    }

    private void Update()
    {
        if (playerIsInBounds)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {

            }
        }
    }
}
