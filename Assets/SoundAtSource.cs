using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAtSource : MonoBehaviour
{
    AudioSource source;
    SoundMaster master;

    public enum SoundClass { UI, MeleeImpact, RangedImpact, Equip, Effect, Voice}
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

        //Assign output channel
        switch (sClass)
        {
            case SoundClass.UI:
                source.outputAudioMixerGroup = master.mg_ui;
                break;
            case SoundClass.Effect:
                source.outputAudioMixerGroup = master.mg_effect;
                source.spatialBlend = 1f;
                break;
            case SoundClass.MeleeImpact:
                source.outputAudioMixerGroup = master.mg_effect;
                break;
            case SoundClass.RangedImpact:
                source.outputAudioMixerGroup = master.mg_effect;
                break;
            case SoundClass.Voice:
                source.outputAudioMixerGroup = master.mg_effect;
                break;

        }

    }

    public void TriggerSound()
    {
        if (sClass == SoundClass.UI)
        {
            //master.PlaySoundAtSource(master.UISource, master.UIgeneral[Random.Range(0, master.UIgeneral.Length -1)]);
            master.PlaySoundAtSourceUI(master.UIgeneral[indexOverride]);
        }
        if(sClass == SoundClass.Effect)
        {
            master.PlaySoundAtSource(source, master.effect[indexOverride]);
        }
        if(sClass == SoundClass.MeleeImpact)
        {
            master.PlaySoundAtSource(source, master.meleeImpacts[indexOverride]);
        }
        if(sClass == SoundClass.Voice)
        {
            master.PlaySoundAtSource(source, master.voice[indexOverride]);
        }
    }

    public void TriggerSoundAtUI()
    {
        if (sClass == SoundClass.Effect)
        {
            master.PlaySoundAtSourceUI(master.effect[indexOverride]);
        }

        if (sClass == SoundClass.Voice)
        {
            master.PlaySoundAtSourceUI(master.voice[indexOverride]);
        }
    }


}
