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
public class zoomies : pAbility
{
    public void monoParser(MonoBehaviour mono)
    {
        mono.StartCoroutine(StartCountdown());
        Debug.Log("Started");
    }
    float currCountdownValue;
    public IEnumerator StartCountdown(float timeLeft = 5f)
    {
        currCountdownValue = timeLeft;
        while (currCountdownValue > 0)
        {
            Debug.Log("Countdown: " + currCountdownValue);
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
            if (currCountdownValue == 0)
            {
                Character.movementSpeed = 5f;
                break;
            }
        }
    }
    public override void pEffect()
    {
        Debug.Log("Ice");
        Character.movementSpeed = 10f;
    }
}
public class aFactory
{
    public static pAbility GpAbility(string aType)
    {
        
        switch (aType)
        {
            case "pushBack":
                pushBack pb = new pushBack();
                pb.pEffect();
                return null;
            case "zoomies":
                zoomies zoom = new zoomies();
                zoom.pEffect();
                return null;
            default:
                return null;
        }
    }
}

