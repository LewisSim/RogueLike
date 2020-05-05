using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMaster : MonoBehaviour
{
    public AudioClip[] meleeImpacts, rangedImpacts, UIgeneral, equip, effect;

    public AudioClip[] music;

    AudioSource musicSource;
    public AudioSource UISource;

    bool playing_music = false;

    private void Awake()
    {
        musicSource = GetComponent<AudioSource>();
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
        source.clip = clip;
        source.Play();
    }
}
