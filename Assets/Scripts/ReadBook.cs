﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReadBook : MonoBehaviour
{

    public GameObject detectObject;
    public GameObject canvasObj;
    public GameObject bookCanvas;
    public Image bookObj;
    public Sprite[] bookSprites;
    public TextMeshProUGUI overlayText;

    private bool reading;
    private int bookPage;


    // Start is called before the first frame update
    void Start()
    {
        reading = false;
        bookPage = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (reading)
        {
            Reading();
        }

        if(detectObject == null)
        {
            detectObject = GameObject.FindGameObjectWithTag("Player");
        }
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject == detectObject)
        {
            AllowRead();
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject == detectObject)
        {
            if (Input.GetButtonDown("Interact"))
            {
                Read(collision);
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject == detectObject)
        {
            HideReadText();
        }
    }

    private void AllowRead()
    {
        ShowInteraction();


    }

    private void Read(Collider collision)
    {

        DisableCharacter();
        reading = true;

        //Audio
        var sas = gameObject.GetComponent<SoundAtSource>();
        sas.indexOverride = 4;
        sas.TriggerSound();
    }

    private void ShowInteraction()
    {
        ShowReadText();
    }

    private void ShowReadText()
    {
        canvasObj.SetActive(true);
        overlayText.GetComponent<TextMeshProUGUI>().isOverlay = true;
    }

    private void HideReadText()
    {
        overlayText.GetComponent<TextMeshProUGUI>().isOverlay = false;
        canvasObj.SetActive(false);
        
    }

    private void DisableCharacter()
    {
        detectObject.gameObject.GetComponent<Character>().enabled = false;
    }

    private void EnableCharacter()
    {
        detectObject.gameObject.GetComponent<Character>().enabled = true;
    }

    private void Reading()
    {
        bookCanvas.SetActive(true);
        bookObj.sprite = bookSprites[bookPage];
        if (Input.GetButton("Cancel"))
        {
            CloseBook();
        }

        if (Input.GetButtonDown("Left"))
        {
            if(bookPage == 0)
            {
                //Do nothing
            }
            else
            {
                bookPage -= 1;
            }
        }

        if (Input.GetButtonDown("Right"))
        {
            if (bookPage == bookSprites.Length - 1)
            {
                //do nothing
            }
            else
            {
                bookPage += 1;
            }
        }
    }

    private void CloseBook()
    {
        bookCanvas.SetActive(false);
        EnableCharacter();
        reading = false;

        //Audio
        var sas = gameObject.GetComponent<SoundAtSource>();
        sas.indexOverride = 3;
        sas.TriggerSound();
    }
}


