using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadBook : MonoBehaviour
{

    public GameObject detectObject;
    public GameObject canvasObj;

    private Vector3 origin;
    private Vector3 direction;
    private float currentHitDistance;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {


    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == detectObject)
        {
            AllowRead();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject == detectObject)
        {
            if (Input.GetButtonDown("Interact"))
            {
                Read();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
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

    private void Read()
    {
        print("im reading");
    }

    private void ShowInteraction()
    {
        ShowReadText();
    }

    private void ShowReadText()
    {
        canvasObj.SetActive(true);
    }

    private void HideReadText()
    {

        canvasObj.SetActive(false);
    }
}


