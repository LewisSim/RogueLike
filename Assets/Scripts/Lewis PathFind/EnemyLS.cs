using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLS : MonoBehaviour
{
    public Rigidbody rb;
    public Transform permaP;

    //Variables
    float lockRange = 2f;
    float minDistance = 10f;
    float Distance;
    Transform nearestTarget = null;
    public Collider[] aiCollider;
    float attackCooldown = 5f;
    bool coolingDown = false;
    public int health = 100;

    // Update is called once per frame
    void Update()
    {
        print(rb.velocity);
        searchEnemy();
    }

    //Combat methods
    void searchEnemy()
    {
        aiCollider = Physics.OverlapSphere(rb.transform.position, lockRange);
        int i = 0;

        while (i < aiCollider.Length)
        {
            if (aiCollider[i].tag == "Player")
            {
                Distance = Vector3.Distance(aiCollider[i].transform.position, rb.transform.position);
                //print(Distance.ToString());
                if (Distance < minDistance)
                {
                    minDistance = Distance;
                    nearestTarget = aiCollider[i].transform;
                    print("Player found");
                }
            }
            i++;
            transform.LookAt(nearestTarget);
            print("Targetting player");
        }
        attackingPlayer();
    }

    void attackingPlayer()
    {
        if (minDistance <= 1.2f && !coolingDown)
        {
            print("Attacking player");
            //nearestTarget.SendMessage("sustainDamage", 50f);
            nearestTarget.SendMessage("sustainNonPureDamage", 50f);
            coolingDown = true;
        }
        else
        {
            attackCooldown -= Time.deltaTime;
            if (attackCooldown <= 0)
            {
                coolingDown = false;
                attackCooldown = 5f;
                print("Complete cooldown");
            }
        }
    }

    void onDeath()
    {

        gameObject.SetActive(false);
    }

    //Methods
    public void AddDamage(int damage)
    {
        if(nearestTarget == null)
        {
            nearestTarget = GameObject.FindGameObjectWithTag("Player").transform;
            gameObject.GetComponent<Unit>().target = nearestTarget;
        }
        health -= damage;
        print(damage.ToString() + " Damage taken!");
        checkHealth();
        gameObject.GetComponent<SoundAtSource>().indexOverride = 7;
        gameObject.GetComponent<SoundAtSource>().TriggerSound();
    }
    public void checkHealth()
    {
        if (health <= 0)
        {
            postDeath();
            gameObject.SetActive(false);
            //Destroy(gameObject);
            gameObject.GetComponent<SoundAtSource>().indexOverride = 6;
            gameObject.GetComponent<SoundAtSource>().TriggerSoundAtUI();
            gameObject.GetComponent<Drop>().DropItem();
        }
    }

    void postDeath()
    {
        print("Post Death");
        nearestTarget.SendMessage("payPlayer", 100);
    }
}
