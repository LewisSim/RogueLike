using System.Collections;
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
    public static float movementSpeed = 5.0f;
    public float rotationSpeed = 200f;

    //Combat variables
    public int attackDam = 10;
    public Collider[] eCollider;
    public Collider[] lCollider;

    private void Start()  
    {
        anim = GetComponent<Animator>();
        Inventory.Instance.TesterMetod();
    }

    //UI Variables
    public Text ui_Gold, ui_Health;

    //Methods
    private void FixedUpdate()
    {
        MovementCheck();
        //Movement();
        Jumping();
        //Mouse camera movement
        // float h = speed * Input.GetAxis("Mouse X");
        // transform.Rotate(0, h, 0);

        //UI Tester
        // ui_Gold.text = Gold.ToString();
        //ui_Health.text = Health.ToString();
    }

    public void Update()
    {
            transform.Rotate(0, Input.GetAxis("Horizontal") * Time.deltaTime * rotationSpeed, 0);
            transform.Translate(0, 0, Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed);

        //Melee attack
        if (Input.GetButtonDown("Fire1"))
        {
            //mAttack();
            //Passing Monobehaviour
            zoomies z = new zoomies();
            z.monoParser(this);
            aFactory.GpAbility("zoomies");
        }
        if (Input.GetMouseButtonDown(2))
        {
            lockOn();
        }

        //aFactory.GpAbility("pushBack");
        //aFactory.GpAbility("zoomies");
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

    public void MovementCheck()
    {
        if ((rb.velocity.x != 0) || (rb.velocity.z != 0))
        {
            anim.SetBool("isWalk", true);
        }
        else
        {
            anim.SetBool("isWalk", false);
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

    //Combat Functions
    public void mAttack()
    {
      float attackRange = 2;
      eCollider = Physics.OverlapSphere(rb.transform.position, attackRange);
        int i = 0;
        while (i < eCollider.Length)
        {
            if (eCollider[i].tag == "Enemy")
            {
                float dT = Vector3.Distance(eCollider[i].transform.position, rb.transform.position);
                if (dT <= attackRange)
                {
                    print(dT.ToString() + " Attack Landed!");
                    eCollider[i].SendMessage("AddDamage", attackDam);
                }
            }
            else if (eCollider[i].tag == "Agent")
            {
                print("AI Boss found!");
                float dT = Vector3.Distance(eCollider[i].transform.position, rb.transform.position);
                if (dT <= attackRange)
                {
                    print(dT.ToString() + " Attack Landed!");
                    eCollider[i].SendMessage("AddDamage", attackDam);
                }
            }
                i++;
        }
    }

    public void lockOn()
    {
        float lockRange = 10;
        float minDistance = 100;
        float Distance;
        Transform nearestTarget = null;
        lCollider = Physics.OverlapSphere(rb.transform.position, lockRange);
        int i = 0;
        while (i < lCollider.Length)
        {
            if (lCollider[i].tag == "Enemy")
            {
                Distance = Vector3.Distance(lCollider[i].transform.position, rb.transform.position);
                print(Distance.ToString());
                if (Distance < minDistance)
                {
                    minDistance = Distance;
                    nearestTarget = lCollider[i].transform;
                }
            }
            i++;
            transform.LookAt(nearestTarget);
        }
        
    }
}
