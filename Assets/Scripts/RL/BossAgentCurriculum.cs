﻿using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using Unity.MLAgents.Sensors;

public class BossAgentCurriculum : Agent
{
    //agent needs
    public bool useVectorObs;
    public GameObject character;
    public GameObject area;
    public GameObject cover1, cover2;
    SphereCollider m_cover1, m_cover2;
    TrainingCharacter m_Character;
    TrainingArea m_Area;
    Rigidbody m_AgentRb;

    //logic needs
    bool m_Attack;
    bool canAttack;
    bool enteredCover;
    bool exitedCover;
    bool inCover;
    bool leftCover;
    bool shouldCover;

    //timers
    float attackTimer = 0f;
    public float attackCooldown;
    private float scrollBar = 1.0f;

    //movements
    public float speed;
    public float maxSpeed;

    EnvironmentParameters m_ResetParams;



    public override void Initialize()
    {
        m_AgentRb = GetComponent<Rigidbody>();
        m_Character = character.GetComponent<TrainingCharacter>();
        m_Area = area.GetComponent<TrainingArea>();
        Time.timeScale = scrollBar;
        m_cover1 = cover1.GetComponent<SphereCollider>();
        m_cover2 = cover2.GetComponent<SphereCollider>();

        m_ResetParams = Academy.Instance.EnvironmentParameters;
        m_Area.PlaceObject(gameObject);


    }


    public override void CollectObservations(VectorSensor sensor)
    {
        if (useVectorObs)
        {
            sensor.AddObservation((transform.InverseTransformDirection(m_AgentRb.velocity)));
            sensor.AddObservation((character.gameObject.transform.position - gameObject.transform.position));
            sensor.AddObservation(cover1.gameObject.transform.position - gameObject.transform.position);
            sensor.AddObservation(cover2.gameObject.transform.position - gameObject.transform.position);
            sensor.AddObservation(m_Character.UsingBallista());
            sensor.AddObservation(canAttack);
            sensor.AddObservation(m_Character.AgentInRange());
            sensor.AddObservation((float)m_Character.health / 100f);
            sensor.AddObservation(enteredCover);
            sensor.AddObservation(leftCover);
            sensor.AddObservation(shouldCover);

        }
    }


    public override void OnActionReceived(float[] vectorAction)
    {

        AddReward(-1f / MaxStep);
        /*
        if (m_Character.UsingBallista())
        {
            CoverAgent(vectorAction);
            //Go To Cover
        }
        else
        {
            AttackAgent(vectorAction);
            //Fight
        }
        */
        CoverAgent(vectorAction);

    }

    public void AttackAgent(float[] act)
    {

        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var forwardAxis = (int)act[0];
        var rotateAxis = (int)act[1];
        var attackAxis = (int)act[2];

        switch (forwardAxis)
        {
            case 1:
                dirToGo = transform.forward * 1f;
                break;
            case 2:
                dirToGo = (transform.forward * -1f) / 2f;
                break;
        }
        switch (rotateAxis)
        {
            case 1:
                rotateDir = transform.up * 1f;
                break;
            case 2:
                rotateDir = transform.up * -1f;
                break;
        }
        switch (attackAxis)
        {
            case 1:
                if (canAttack)
                {
                    m_Attack = true;
                }
                else
                {
                    AddReward(-0.01f); //punish for trying to attack when cooldown is not over
                    m_Attack = false;
                }
                break;
        }



        transform.Rotate(rotateDir, Time.deltaTime * 200f);
        m_AgentRb.velocity = dirToGo * speed;

        //been told to attack
        if (m_Attack)
        {
            //if we are in range
            if (m_Character.AgentInRange())
            {
                Attack();
            }
            else
            {
                AddReward(-0.05f); //punish for attacking without being in range
            }

            m_Attack = false;
            attackTimer = attackCooldown;
        }
    }

    public void CoverAgent(float[] act)
    {

        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var forwardAxis = (int)act[0];
        var rotateAxis = (int)act[1];
        var attackAxis = (int)act[2];

        switch (forwardAxis)
        {
            case 1:
                dirToGo = transform.forward * 1f;
                break;
            case 2:
                dirToGo = (transform.forward * -1f) / 2f;
                break;
        }
        switch (rotateAxis)
        {
            case 1:
                rotateDir = transform.up * 1f;
                break;
            case 2:
                rotateDir = transform.up * -1f;
                break;
        }
        switch (attackAxis)
        {
            case 1:
                if (canAttack)
                {
                    m_Attack = true;
                }
                else
                {
                    AddReward(-0.01f); //punish for trying to attack when cooldown is not over
                    m_Attack = false;
                }
                break;
        }



        transform.Rotate(rotateDir, Time.deltaTime * 200f);
        m_AgentRb.velocity = dirToGo * speed;

        //been told to attack
        if (m_Attack)
        {
            AddReward(-0.75f); // punish for attacking when we should be covering

            m_Attack = false;
            Timer(attackTimer, attackCooldown);
        }
    }

    private void Update()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer > attackCooldown)
        {
            canAttack = true;

        }
        else
        {
            canAttack = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {


        if (m_AgentRb.velocity.magnitude > maxSpeed)
        {
            m_AgentRb.velocity = Vector3.ClampMagnitude(m_AgentRb.velocity, maxSpeed);
        }


    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("cover"))
        {
            GotCover();

        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("cover"))
        {
            LeftCover();

        }
    }

    void GotCover()
    {
        leftCover = false;
        enteredCover = true;
        print("got cover");
        if (m_Character.UsingBallista())
        {
            AddReward(1f);
            shouldCover = true;
            print(" in cover");
        }
        else
        {
            AddReward(-0.25f);
            print("shouldnt be in cover");
        }
    }

    void LeftCover()
    {
        enteredCover = false;
        leftCover = true;
        if (m_Character.UsingBallista())
        {
            AddReward(-0.5f);
        }
        else
        {
            if (shouldCover)
            {
                AddReward(2f);
                shouldCover = false;
                EndEpisode();
            }

        }
    }



    void Attack()
    {
        AddReward(0.5f); //reward for attacking the character
        m_Character.health -= 10;

        if (m_Character.health <= 0)
        {
            AddReward(1f);
            EndEpisode();
        }
    }

    void Timer(float t, float c)
    {
        t = c;
    }

    public override void OnEpisodeBegin()
    {
        leftCover = false;
        enteredCover = false;
        m_Attack = false;
        attackTimer = 0f;
        m_Character.ResetValues();
        m_AgentRb.velocity = Vector3.zero;
        transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));
        m_Area.PlaceObject(gameObject);
        m_cover1.radius = m_ResetParams.GetWithDefault("sphereRadius", 0.5f);
        m_cover2.radius = m_ResetParams.GetWithDefault("sphereRadius", 0.5f);
        
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0;
        actionsOut[1] = 0;
        actionsOut[2] = 0;
        if (Input.GetKey(KeyCode.D))
        {
            actionsOut[1] = 2f;
        }
        if (Input.GetKey(KeyCode.W))
        {
            actionsOut[0] = 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            actionsOut[1] = 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            actionsOut[0] = 2f;
        }
        actionsOut[2] = Input.GetKey(KeyCode.Space) ? 1.0f : 0.0f;


    }
}
