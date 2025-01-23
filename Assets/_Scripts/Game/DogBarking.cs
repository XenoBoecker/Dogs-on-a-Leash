using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class DogBarking : MonoBehaviour
{
    public string dogSoundsPath = "Sound/SoundEffects/DogSFX/dog1_bark";
    private AudioClip[] dogBarkSounds;

    [SerializeField] VisualEffect barkEffect;

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

    // Make the dogs go b a r k
    private void PlayRandomDogBark()
    {
        barkEffect.Play();
        SoundManager.Instance.PlaySound(dogBarkSounds);
    }
}
