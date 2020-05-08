using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LootChest : MonoBehaviour
{

    //public GameObject player;

    public GameObject txtCanvas;
    public TextMeshProUGUI overlayText;

    bool playerIsInBounds = false;

    Drop dropScript;
    // Start is called before the first frame update
    void Start()
    {
        dropScript = GetComponent<Drop>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            txtCanvas.SetActive(true);
            overlayText.GetComponent<TextMeshProUGUI>().isOverlay = true;
            playerIsInBounds = true;
            //Open();
        }
    }

    void Open()
    {
        dropScript.DropItem();
        Destroy(gameObject);
    }




    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
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
                Open();
                gameObject.GetComponent<SoundAtSource>().TriggerSoundAtUI();
            }
        }
    }
}
