using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAtSource : MonoBehaviour
{
    AudioSource source;
    SoundMaster master;

    public enum SoundClass { UI, MeleeImpact, RangedImpact, Equip, Effect}
    public SoundClass sClass;

    public int indexOverride;

    // Start is called before the first frame update
    void Start()
    {

        //Add audio source if none
        if (!gameObject.GetComponent<AudioSource>())
        {
            source = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            source = gameObject.GetComponent<AudioSource>();
        }

        master = GameObject.FindGameObjectWithTag("SoundSystem").GetComponent<SoundMaster>();


    }

    public void TriggerSound()
    {
        if (sClass == SoundClass.UI)
        {
            master.PlaySoundAtSource(master.UISource, master.UIgeneral[indexOverride]);
        }
        if(sClass == SoundClass.Effect)
        {
            master.PlaySoundAtSource(source, master.effect[indexOverride]);
        }
        if(sClass == SoundClass.MeleeImpact)
        {
            master.PlaySoundAtSource(source, master.meleeImpacts[indexOverride]);
        }
    }

    public void TriggerSoundAtUI()
    {
        if (sClass == SoundClass.Effect)
        {
            master.PlaySoundAtSource(master.UISource, master.effect[indexOverride]);
        }
    }


}
