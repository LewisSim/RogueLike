using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    public GameObject Enemy;
    public float waitSecs = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        while (waitSecs < 2) 
        {
            waitSecs += Time.deltaTime;
        }

        if(waitSecs > 2) //ensures everything is spawned in
        {
            Enemy = GameObject.FindGameObjectWithTag("Enemy");
        }

        RaycastHit hit;

        if(Physics.Raycast(Enemy.transform.position, Enemy.transform.forward, out hit, 10f))
        {
            if(hit.transform.gameObject.tag == "Player")
            {
                gameObject.GetComponent<Grid>().enabled = true;
                gameObject.GetComponent<Pathfind>().enabled = true;
            }
        }

    }
}
