using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCStatement : MonoBehaviour
{

    public LayerMask layerMask;
    public float sphereRadius;
    public float maxDistance;
    public GameObject canvasObj;
    public Camera camera;
    public GameObject text;
    public string statementText;
    public float timer;

    private Vector3 origin;
    private Vector3 direction;
    private TextMeshProUGUI tmprotext;
    private float textTimer;


    private float currentHitDistance;

    // Start is called before the first frame update
    void Start()
    {
        tmprotext = text.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        textTimer -= Time.deltaTime;
        origin = transform.position;
        direction = transform.forward;

        RaycastHit hit;

        if (Physics.SphereCast(origin,sphereRadius, direction, out hit, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal))
        {
            currentHitDistance = hit.distance;
            Diagloue();
        }
        else
        {
            currentHitDistance = maxDistance;
        }

        if ((textTimer < 0) && (tmprotext.text == statementText)){
            tmprotext.SetText("");
        }



        FaceCamera();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(origin, origin + direction * currentHitDistance);
        Gizmos.DrawWireSphere(origin + direction * currentHitDistance, sphereRadius);
    }

    private void Diagloue()
    {
        tmprotext.SetText(statementText);
        textTimer = timer;
    }

    private void FaceCamera()
    {
        canvasObj.transform.rotation = Quaternion.LookRotation(transform.position - camera.transform.position);
    }
}
