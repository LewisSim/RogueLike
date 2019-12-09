using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    //Variables
    public Rigidbody rb;
    public float movementSpeed = 10f;
    public float jumpHeight = 15;
    float maxVelocity = 3;
    bool isGrounded, isJumping;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    //Methods
    private void FixedUpdate()
    {
        //check if we stopped moving
        if((rb.velocity.x != 0) || (rb.velocity.z != 0))
        {
            anim.SetBool("isWalk", true);
        }
        else
        {
            anim.SetBool("isWalk", false);
        }
        Movement();
        Jumping();
    }

    public void Movement()
    {
        //Walking
        rb.velocity = new Vector3((Input.GetAxis("Horizontal") * movementSpeed), rb.velocity.y, Input.GetAxis("Vertical") * movementSpeed);
        Vector3 movement = new Vector3((Input.GetAxis("Horizontal") * movementSpeed), rb.velocity.y, Input.GetAxis("Vertical") * movementSpeed);
        rb.velocity = Vector3.ClampMagnitude(movement, maxVelocity);
    }

    public void Jumping()
    {

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            if (hit.distance < 0.50f)
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
        float jumpForce = 10;

        if (isGrounded && Input.GetKey(KeyCode.Space))
        {
            isJumping = true;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        else if ((isJumping) && (isGrounded))
        {
            isJumping = false;
        }

    }

}
