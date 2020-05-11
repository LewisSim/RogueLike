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
    public GameObject reticle;
    public Camera cam;
    public GameObject character;
    public bool usingBallista;
    public GameObject ballista;

    public float mouseSensitivity = 1;
    public Vector2 pitchMinMax = new Vector2(-40, 85);
    public float rotationSmoothTime = .12f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;
    float yaw;
    float pitch;
    public Vector3 aY;
    private Vector3 velocity = Vector3.zero;
    public GameObject dum;


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

    private void FixedUpdate()
    {

    }

    private void Update()
    {
        if (playerIsInBounds)
        {
            if (Input.GetKeyDown(KeyCode.E) && (!usingBallista))
            {
                usingBallista = true;
                player.SetActive(false);
                reticle.gameObject.SetActive(true);
                cam.gameObject.SetActive(true);
                character.SetActive(true);
                txtCanvas.SetActive(false);
                overlayText.GetComponent<TextMeshProUGUI>().isOverlay = true;
            }
        }

        if (usingBallista)
        {
            rAttack();
        }
    }


    public void rAttack() //Ranged
    {
        /*
        var newY = dum.transform.localPosition.y + Input.GetAxis("Mouse Y");
        newY = Mathf.Clamp(newY, 1f, 3.5f);
        //aY = new Vector3(0, newY * 1, 0);
        aY = Vector3.SmoothDamp(dum.transform.localPosition, new Vector3(dum.transform.localPosition.x, newY, dum.transform.localPosition.z), ref velocity, 0.3f);
        dum.transform.localPosition = aY;
        cam.transform.LookAt(dum.transform);

    */

        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -15f, 5f);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(0, yaw, pitch), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
            
        }

    }
    void Shoot()
    {
        float RattackRange = 100f;
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, RattackRange))
        {
            print("WorkingG");
            //hit.collider.SendMessage("AddDamage", rangedAttackDam);
            if (hit.transform.name == "agent")
            {
                //hit.collider.SendMessage("AddDamage", rangedAttackDam);
            }
        }
    }

}
