using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlledVolumeAudioSource : MonoBehaviour
{
    AudioSource source;


    [SerializeField] float volumeMultiplier = 1;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();

        SoundManager.Instance.OnSoundReload += SoundReload;

        SoundReload();
    }

    private void SoundReload()
    {
        source.volume = SoundManager.Instance.SFXVolume * volumeMultiplier;
    }
}
