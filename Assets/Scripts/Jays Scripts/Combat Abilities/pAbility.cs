using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstract player ability class
public abstract class pAbility
{
    public abstract void pEffect();
}

public class pushBack : pAbility
{
    GameObject Player = GameObject.FindWithTag("Player");
    public Collider[] lCollider;
    float pbRange = 10;
    float minDistance = 100;
    float Distance;
    float force = 5;
    public override void pEffect()
    {
        Debug.Log("Push back - working");
        lCollider = Physics.OverlapSphere(Player.transform.position, pbRange);
        int i = 0;
        while (i < lCollider.Length)
        {
            if (lCollider[i].tag == "Enemy")
            {
                //lCollider[i].GetComponent<Rigidbody>().velocity *= 2;
                lCollider[i].GetComponent<Rigidbody>().AddForce(lCollider[i].transform.position * force);
            }
            i++;
        } 
    }
}

public class aFactory
{
    public static pAbility GpAbility(string aType)
    {
        pushBack pb = new pushBack();
        switch (aType)
        {
            case "pushBack":
                pb.pEffect();
                return null; 
            default:
                return null;
        }
    }
}

