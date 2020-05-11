using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
public class Character : MonoBehaviour
{
    //Variables
    public Rigidbody rb;
    public float jumpHeight = 15;
    public float speed = 2f;
    public float Health, Gold;
    float maxVelocity = 3;
    bool isGrounded, isJumping, isAiming;
    Animator anim;
    public static float movementSpeed = 4.0f;
    public float rotationSpeed = 200f;
    public Image reticle;

    //Camera variables
    public float lookSpeed = 3;
    private Vector2 rotation = Vector2.zero;
    bool LockedOn;

    //Combat variables
    public int attackDam = 10;
    public int rangedAttackDam = 20;
    public float cooldown = 2f;
    public bool coolingdown = false;
    //
    public float cooldown2 = 10f;
    public bool coolingdown2 = false;
    public bool attackFinished = false;

    //public float attackRange;
    public Collider[] eCollider;
    public Collider[] lCollider;
    public Camera cam;

    //Camera variables
    public bool lockCursor;
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

    //Jumping Variables 
    public float fallMultiplier = 2.5f;
    public float lowJumperMultiplier = 2f;
    public Vector3 jump = new Vector3(0.0f, 2.0f, 0.0f);

    //UI Variables
    public TextMeshProUGUI ui_Gold, ui_Health;
    public Image deathScreen;
    public HealthBarScript healthBarScript;

    //Analytic Vars
    float timeToSend;
    int m_UserID;
    int m_Time = 1;
    string m_weaponPref;
    string urlset = "http://daredicing.com/setUserStats.php";


    //Methods 
    private void Start()
    {
        anim = GetComponent<Animator>();
        //Inventory.Instance.TesterMetod();
        LockedOn = false;
        StartCoroutine(CameraSwitch());
        Cursor.lockState = CursorLockMode.Locked;
        getUserID();
        timeToSend = 60f;
        Health = 100;
        ui_Gold.text = "Gold: ";
    }

    private void FixedUpdate()
    {
        MovementCheck();
        Jumping();
        print(LockedOn);

        //After A Minute Do Our GameAnalytic Function
        timeToSend -= Time.deltaTime;
        if (timeToSend < 0)
        {
            StartCoroutine(SendData());
        }

        var newY = dum.transform.localPosition.y + Input.GetAxis("Mouse Y");
        newY = Mathf.Clamp(newY, 1f, 3.5f);
        //aY = new Vector3(0, newY * 1, 0);
        aY = Vector3.SmoothDamp(dum.transform.localPosition, new Vector3(dum.transform.localPosition.x, newY, dum.transform.localPosition.z), ref velocity, 0.3f);
        dum.transform.localPosition = aY;
        cam.transform.LookAt(dum.transform);

        AbilityExecution();

        ui_Gold.text = "Gold: " + Gold.ToString();
    }
    public void Update()
    {
        //transform.Rotate(0, Input.GetAxis("Horizontal") * Time.deltaTime * rotationSpeed, 0);
        // transform.Translate(0, 0, Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed);
        Movement();
        //Melee attack
        if (Input.GetButtonDown("Fire1") && isAiming == false)
        {
            anim.SetTrigger("isMAttacking");
            //mAttack();
        }
        //Ranged attack
        rAttack();
        //Switching Camera
        if (Input.GetMouseButtonDown(2))
        {
            LockedOn = !LockedOn;
        }


        //USE POTION
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UsePotion();
        }
    }
    IEnumerator CameraSwitch()
    {
        while (true)
        {
            if (LockedOn == false)
            {
                print("lockedonfalse");
                yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
                currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(0, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
                transform.eulerAngles = currentRotation;
                //yield return null;
            }
            if (LockedOn == true)
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
            yield return null;
        }
    }
    IEnumerator CameraLook()
    {
        print("active");
        while (true)
        {
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            //pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            //pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

            currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(0, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
            transform.eulerAngles = currentRotation;
            yield return null;
        }
    }

    public void Movement()
    {
        var localVelocity = new Vector3((Input.GetAxis("Horizontal") * movementSpeed), rb.velocity.y, Input.GetAxis("Vertical") * movementSpeed);
        //Vector3 movement = new Vector3((Input.GetAxis("Horizontal") * movementSpeed), 0, Input.GetAxis("Vertical") * movementSpeed);
        rb.velocity = transform.TransformDirection(localVelocity);
        //rb.velocity = Vector3.ClampMagnitude(transform.TransformDirection(localVelocity), movementSpeed);
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
        float jumpForce = 2f;

        if (isGrounded && Input.GetKey(KeyCode.Space))
        {
            if (!isJumping)
            {
                isJumping = true;
                //rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                //rb.velocity += Vector3.up * Physics.gravity.y * 500 * Time.deltaTime;
                rb.AddForce(jump * jumpForce, ForceMode.Impulse);
                isGrounded = false;
                Invoke("resetIsJumping", 1.5f);
            }
        }
    }

    private void resetIsJumping()
    {
        isJumping = false;
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
    public void mAttack() //Melee
    {

        float tmpAttVal;

        if (Inventory.p_inventory[0] == null)
        {

            tmpAttVal = 10;
        }
        else
        {
            tmpAttVal = Inventory.p_inventory[0].Damage;
        }

        var newDam = attackDam + ((tmpAttVal * 1.5));
        print("New damage " + newDam);

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
                    eCollider[i].SendMessage("AddDamage", newDam);
                    //anim.SetBool("isMAttacking", false);
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
    public void rAttack() //Ranged
    {
        if (Input.GetMouseButtonDown(1) && isAiming == false)
        {
           // print("RMBd active");
            reticle.gameObject.SetActive(true);
            isAiming = true;
        }
        else if (Input.GetMouseButtonDown(1) && isAiming == true)
        {
            //print("RMBd deactivated");
            reticle.gameObject.SetActive(false);
            isAiming = false;
        }
        //Actual firing mechanic
        if (isAiming == true)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
               print("Firing MY G");
            }
        }
    }
    void Shoot()
    {
        float RattackRange = 300f;
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, RattackRange))
        {
            print("WorkingG");
            Debug.Log(hit.transform.name);
            //hit.collider.SendMessage("AddDamage", rangedAttackDam);
            if (hit.transform.name == "minion@idles")
            {
                hit.collider.SendMessage("AddDamage", rangedAttackDam);
            }
        }
    }
    IEnumerator lockOn()
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
            yield return null;
        }
    }
    //Special abilities 
    void abOne()
    {
        aFactory.GpAbility("pushBack");
    }
    void abTwo()
    {
        zoomies z = new zoomies();
        z.monoParser(this);
        aFactory.GpAbility("zoomies");
    }

    //Ability execution
    void AbilityExecution()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1) && !coolingdown)
        {
            abOne();
            coolingdown = true;
           // print("Executed");
        }
        else
        {
            cooldown -= Time.deltaTime;
            if (cooldown <= 0)
            {
                coolingdown = false;
                cooldown = 2f;
                //print("Cooling down");
            }
        }
        //
        if (Input.GetKeyDown(KeyCode.Alpha2) && !coolingdown2)
        {
            abTwo();
            coolingdown2 = true;
            //print("Executed2");
        }
        else
        {
            cooldown2 -= Time.deltaTime;
            if (cooldown2 <= 0)
            {
                coolingdown2 = false;
                cooldown2 = 2f;
                //print("Cooling down2");
            }
        }
    }
    //Analytic Functions
    void getUserID()
        {
            m_UserID = PlayerPrefs.GetInt("UserID");
        }

        IEnumerator SendData()
        {
            timeToSend = 60f;
            WWWForm form = new WWWForm();
            form.AddField("userID", m_UserID.ToString());
            form.AddField("sendHealth", Health.ToString());
            form.AddField("sendTime", m_Time.ToString());
            form.AddField("sendWeaponPref", m_weaponPref.ToString());

            using (UnityWebRequest www = UnityWebRequest.Post(urlset, form))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    print(www.error);
                }
                else
                {
                    print("Form upload complete!");
                }
            }
    }
    //Combat detriment functions 
    public void sustainDamage(float damageTaken)
    {
        Health = Health - damageTaken;

        print("Player has sustained damage");

        healthBarScript.health.CurrentVal = Health;

        //Death check
        if (Health <= 0)
        {
            print("Player is now dead");
            playerDead();
        }
    }

    public void sustainNonPureDamage(float damageTaken)
    {
        //Tester vars
        float tmpArmourVal;

        if (Inventory.p_inventory[4] == null)
        {

            tmpArmourVal = 10;
        }
        else
        {
            tmpArmourVal = Inventory.p_inventory[4].ArmourRating;
        }

        var newDam = damageTaken - ((tmpArmourVal * 1));
        print("New damage" + newDam);
        Health = Health - newDam;
        print("New Health" + Health);


        healthBarScript.health.CurrentVal = Health;

        //Death check
        if (Health <= 0)
        {
            playerDead();
        }
    }

    public void UsePotion()
    {
        if (Inventory.p_inventory[3] != null)
        {
            var sound = GetComponent<SoundAtSource>();
            switch (Inventory.p_inventory[3].SubType)
            {
                case Item.ItemSubType.HealthPot:
                    healthBarScript.health.CurrentVal += Inventory.p_inventory[3].Potency * 40;
                    sound.indexOverride = 8;
                    break;
                case Item.ItemSubType.DamagePot:
                    sound.indexOverride = 10;
                    break;
                case Item.ItemSubType.SpeedPot:
                    sound.indexOverride = 11;
                    break;
            }

            sound.TriggerSoundAtUI();
            Inventory.RemoveItem(3);
            Inventory.UpdateUI();

        }
    }

    public void playerDead()
    {
        deathScreen.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void payPlayer(int payment)
    {
        print(Gold);
        Gold += payment;
        print(Gold);
    }
}
