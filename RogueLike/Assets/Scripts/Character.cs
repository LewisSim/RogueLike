using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    //Variables
    public Rigidbody rb;
    public float movementSpeed = 3f;
    public float jumpHeight = 15;

    //Methods
    private void FixedUpdate()
    {
        Movement();
    }

    public void Movement()
    {
        //float moveHorizontal = Input.GetAxis("Horizontal");
        //float moveVertical = Input.GetAxis("Vertical");
        //Not initially required (JAY) Vector3 verticalMovement = moveVertical * rb.transform.forward;
        //rb.velocity = new Vector3((moveHorizontal * speed), rb.velocity.y, moveVertical * speed);

        rb.velocity = new Vector3((Input.GetAxis("Horizontal") * movementSpeed), rb.velocity.y, Input.GetAxis("Vertical") * movementSpeed);

    }

}
