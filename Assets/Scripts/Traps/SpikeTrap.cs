using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public Vector3 untriggeredPos, triggeredPos;
    public GameObject spikes;
    public int damage;
    public float timerMax;
    private bool isTriggered;
    private float timer = 0f;

    private void OnTriggerEnter(Collider other)
    {
        Trigger(other.transform.gameObject);
    }

    private void Update()
    {
        if (isTriggered)
        {
            if(timer < timerMax)
            {
                timer += Time.deltaTime;
            }
            if(timer >= timerMax)
            {
                timer = 0f;
                isTriggered = false;
                spikes.transform.localPosition = untriggeredPos;
            }
        }
    }

    public void Trigger(GameObject hit)
    {
        spikes.transform.localPosition = triggeredPos;
        isTriggered = true;
        if (hit.GetComponent<Character>())
        {
            hit.SendMessage("sustainDamage", damage);
        }
        gameObject.GetComponent<SoundAtSource>().TriggerSound();
    }
}
