using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCConvosation : MonoBehaviour
{

    public LayerMask layerMask;
    public float sphereRadius, distance;
    public GameObject text;
    public string[] npcDialogue;
    public bool shouldCharacterRespond;
    public string[] responses;

    private Vector3 origin;
    private Vector3 direction;
    private TextMeshProUGUI tmprotext;
    private float currentHitDistance;

    // Start is called before the first frame update
    void Start()
    {
        tmprotext = text.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        origin = transform.position;
        direction = transform.forward;

        RaycastHit hit;

        if (Physics.SphereCast(origin, sphereRadius, direction, out hit, distance, layerMask, QueryTriggerInteraction.UseGlobal))
        {
            currentHitDistance = hit.distance;
            Diagloue();
        }
        else
        {
            currentHitDistance = distance;
        }


    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(origin, origin + direction * currentHitDistance);
        Gizmos.DrawWireSphere(origin + direction * currentHitDistance, sphereRadius);
    }

    private void Diagloue()
    {

    }

}
