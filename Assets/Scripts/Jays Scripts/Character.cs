﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Character : MonoBehaviour
{

    //Variables
    public Rigidbody rb;
   // public float movementSpeed = 10f;
    public float jumpHeight = 15;
    public float speed = 2f;
    public int Health, Gold;
    float maxVelocity = 3;
    bool isGrounded, isJumping;
    Animator anim;
    public float movementSpeed = 5.0f;
    public float rotationSpeed = 100.0f;

    private void Start()  
    {
        usePowerUp();
        anim = GetComponent<Animator>();
    }

    //UI Variables
    public Text ui_Gold, ui_Health;

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
        //Movement();
        Jumping();

        //Mouse camera movement
       // float h = speed * Input.GetAxis("Mouse X");
       // transform.Rotate(0, h, 0);

        //UI Tester
        //ui_Gold.text = Gold.ToString();
        //ui_Health.text = Health.ToString();
    }

    public void Update()
    {
            transform.Rotate(0, Input.GetAxis("Horizontal") * Time.deltaTime * rotationSpeed, 0);
            transform.Translate(0, 0, Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed);
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
        float jumpForce = 1f;

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

    public void usePowerUp()
    {
        PowerUpGold pwrup1 = new PowerUpGold();
        Gold = pwrup1.alterBehaviour(Gold);
        print(Gold);

        PowerUpHealth pwrup2 = new PowerUpHealth();
        Health = pwrup2.alterBehaviour(Health);
        print(Health);
    }
}
