using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spherecasting : MonoBehaviour
{
    public GameObject currHit;

    public float radius;
    public float maxdist;
    public LayerMask layermask;

    private Vector3 origin;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        origin = transform.position;
        Collider[] hitColliders = Physics.OverlapSphere(origin, radius);

        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].transform.tag == "Player")
            {
                gameObject.GetComponent<EnemyPath>().enabled = true;
            }

            i++;
        }
    }
}
