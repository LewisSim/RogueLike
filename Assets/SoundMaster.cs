using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMaster : MonoBehaviour
{
    public AudioClip[] meleeImpacts, rangedImpacts, UIgeneral, equip, effect, voice;

    public AudioClip[] music;

    AudioSource musicSource;
    public AudioSource[] UISource;
    public GameObject UISFX;

    public AudioMixer masterAudioMixer;
    public AudioMixerGroup mg_ui, mg_effect, mg_ambient, mg_music;

    bool playing_music = false;

    private void Awake()
    {
        musicSource = GetComponent<AudioSource>();
        UISource = UISFX.GetComponents<AudioSource>();
    }

    private void Update()
    {


    }

    public void StartMusic()
    {
        musicSource.Play();
        playing_music = true;
    }

    public void StopMusic()
    {
        musicSource.Stop();
        playing_music = false;
    }

    public void SetMusic(AudioClip clip)
    {
        musicSource.clip = clip;
    }

    public void PlaySoundAtSource(AudioSource source, AudioClip clip)
    {
        if (!source.isPlaying)
        {
            source.clip = clip;
            source.Play();
        }
    }

    public void PlaySoundAtSourceUI(AudioClip clip)
    {
        //Debug.Log("in heere 1");
        foreach (var source in UISource)
        {
            //Debug.Log("in heere 2");
            if (!source.isPlaying)
            {
                source.clip = clip;
                source.Play();
                //Debug.Log("source: " + source);
                return;
            }
        }   
    }
}
