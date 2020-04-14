using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeTrapParentScript : MonoBehaviour
{
    public AxeTrap[] axes;
    public bool useTimer;
    public float timer;
    float count;

    private void Update()
    {
        if (useTimer)
        {
            count += Time.deltaTime;

            if(count >= timer)
            {
                foreach (var axe in axes)
                {
                    axe.TriggerToggleSwinging();
                }
            }
        }
    }
}
