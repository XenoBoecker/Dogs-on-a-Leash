using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class DogBarking : MonoBehaviour
{
    [SerializeField] VisualEffect barkEffect;

    Dog dog;

    void Awake()
    {
        barkEffect.Stop();

        dog = GetComponent<Dog>();
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

        if (dog.DogData.id == 0) SoundManager.Instance.PlaySound(SoundManager.Instance.dogSFX.barkBernard);
        else if (dog.DogData.id == 1) SoundManager.Instance.PlaySound(SoundManager.Instance.dogSFX.barkPoodle);
        else if (dog.DogData.id == 2) SoundManager.Instance.PlaySound(SoundManager.Instance.dogSFX.barkPug);
        else if (dog.DogData.id == 3) SoundManager.Instance.PlaySound(SoundManager.Instance.dogSFX.barkRetreiver);
    }
}
