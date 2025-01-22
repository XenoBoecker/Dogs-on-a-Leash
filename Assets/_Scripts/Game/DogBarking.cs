using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;
using UnityEngine.InputSystem;

public class DogBarking : MonoBehaviour
{
    public string dogSoundsPath = "Sound/SoundEffects/DogSFX/dog1_bark";
    private AudioClip[] dogBarkSounds;

    void Awake()
    {
        dogBarkSounds = Resources.LoadAll<AudioClip>(dogSoundsPath);
    }

    void OnEnable()
    {
        GetComponent<PlayerDogController>().OnBark += PlayRandomDogBark;
    }

    private void OnDisable()
    {
        GetComponent<PlayerDogController>().OnBark -= PlayRandomDogBark;
    }
    private void PlayRandomDogBark()
    {
        SoundManager.Instance.PlaySound(dogBarkSounds);
    }
}
