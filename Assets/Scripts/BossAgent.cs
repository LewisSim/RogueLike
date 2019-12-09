using MLAgents;
using UnityEngine;

public class BossAgent : Agent
{

    public float speed = 10; // speed of the boss
    Transform target; //the reward box
    GameObject[] covers;
    Rigidbody rBody;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        GameObject theCover = FindClosestCover();
        target = theCover.transform;
    } 
    private GameObject FindClosestCover()
    {
    covers = GameObject.FindGameObjectsWithTag("Cover");
    GameObject closest = null;
    float distance = Mathf.Infinity;

    Vector3 position = transform.position;
        foreach (GameObject cover in covers)
    {
        Vector3 diff = cover.transform.position - position;
        float curDistance = diff.sqrMagnitude;
        if (curDistance < distance)
        {
            closest = cover;
            distance = curDistance;
        }
    }
    return closest;
    }

    public override void AgentReset()
    {
        // If we fall out of the world
        if (this.transform.position.y < 0)
        {
            // If the Agent fell, zero its momentum
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.position = new Vector3( 0, 0.5f, 0);
        }

        // Move the target to a new spot
        target.position = new Vector3(Random.value * 8 - 4,
            0.5f,
            Random.value * 8 - 4);
        
        GameObject theCover = FindClosestCover();
        target = theCover.transform;
    }
    
    public override void CollectObservations()
    {
        // Target and Agent positions
        AddVectorObs(target.position);
        AddVectorObs(this.transform.position);

        // Agent velocity
        AddVectorObs(rBody.velocity.x);
        AddVectorObs(rBody.velocity.z);
    }
    
    
    public override void AgentAction(float[] vectorAction, string textAction)
    {
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = vectorAction[1];
        rBody.AddForce(controlSignal * speed);

        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.position,
            target.position);

        // Reached target
        if (distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            Done();
        }

        // Fell off platform
        if (this.transform.position.y < 0)
        {
            Done();
        }

    }
    
    public override float[] Heuristic()
    {
        var action = new float[2];
        action[0] = Input.GetAxis("Horizontal");
        action[1] = Input.GetAxis("Vertical");
        return action;
    }
}
