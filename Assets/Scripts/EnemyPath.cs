using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{

    private Transform goToPoint, playerPos; //add the beacon
    [SerializeField]
    private float speed = 0.2f;

    Vector3[] path /*= new Vector3[1]*/;
    int targetIndex;

    Animator state;

    private void Start()
    {
        state = GetComponent<Animator>();
        

        goToPoint = GameObject.FindGameObjectWithTag("Player").transform;
        ReqManager.RequestPath(transform.position, goToPoint.position, OnPathFound);//create path 

    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider collision) 
    {
        if (collision.gameObject.tag == "beacon")
        {
            state.SetBool("inRange", true);
            hitBeacon();
        }

    }
    private void hitBeacon()
    {

    }





    public void OnPathFound(Vector3[] newPath, bool pathSucessful)
    {
        if (pathSucessful)
        {

            path = newPath;

            for (int i = 0; i < path.Length; i++)//=
            {

            }

            StopCoroutine("followPath");
            StartCoroutine("followPath");
        }
        else
        {
        }

    }


    IEnumerator followPath()
    {

        if (targetIndex >= path.Length)//reset
        {

            targetIndex = 0;
        }

        //targetIndex = 0;

        Vector3 currentWaypoint = path[0];//0 //set start

        while (true)
        {


            if (transform.position == currentWaypoint)
            {
                //Debug.Log(currentWaypoint[1]);/////////
                targetIndex++;//move to next
                if (targetIndex >= path.Length)
                {
                    state.SetBool("inRange", true);
                    hitBeacon();
                    //Debug.Log(state.GetBool("InRange"));



                    targetIndex = 0;///////
                    yield break;//stop
                }
                currentWaypoint = path[targetIndex];// update to next waypoint

            }

            if (currentWaypoint == path[path.Length - 1])
            {
        
            }
            else
            {
            }
            
            transform.LookAt(currentWaypoint);
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, 0.03f);////speed
            yield return null;
        }
    }
}
