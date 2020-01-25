using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCStatement : MonoBehaviour
{

    public LayerMask layerMask;
    public float sphereRadius, distance, timer;
    public GameObject canvasObj, text;
    public Camera camera;
    public bool randomiseStatement;
    public string[] randomStatementText;
    public string statementText;

    private Vector3 origin, direction;
    private TextMeshProUGUI tmprotext;
    private float currentHitDistance, textTimer;
    private int chooseRandom;

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

        if (Physics.SphereCast(origin,sphereRadius, direction, out hit, distance, layerMask, QueryTriggerInteraction.UseGlobal))
        {
            currentHitDistance = hit.distance;
            Diagloue();
        }
        else
        {
            currentHitDistance = distance;
        }

        if ((textTimer < 0) && (tmprotext.text != "")){
            tmprotext.SetText("");
        }

        CameraFace(canvasObj, camera);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(origin, origin + direction * currentHitDistance);
        Gizmos.DrawWireSphere(origin + direction * currentHitDistance, sphereRadius);
    }

    private void Diagloue()
    {
        if (randomiseStatement)
        {
            if ((textTimer < 0))
            {
                RandomStatement();
            }        
            tmprotext.SetText(randomStatementText[chooseRandom]);
        }
        else
        {
            tmprotext.SetText(statementText);          
        }
        textTimer = timer;
    }

    private void CameraFace(GameObject obj, Camera camera)
    {
        obj.transform.rotation = Quaternion.LookRotation(transform.position - camera.transform.position);
    }

    private void RandomStatement()
    {
        chooseRandom = Random.Range(0, randomStatementText.Length);
    }

}
