using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class DogBarking : MonoBehaviour
{
    [SerializeField] VisualEffect barkEffect;

    Dog dog;

    float barkWaitTimer;

    public event Action OnBark;

    void Awake()
    {
        barkEffect.Stop();

        dog = GetComponent<Dog>();
    }

    private void Update()
    {
        barkWaitTimer -= Time.deltaTime;
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
        if (barkWaitTimer > 0){
            return;
        }

        barkEffect.Play();

        if (dog.DogData.id == 0) barkWaitTimer = SoundManager.Instance.PlaySound(SoundManager.Instance.dogSFX.barkBernard).length;
        else if (dog.DogData.id == 1) barkWaitTimer = SoundManager.Instance.PlaySound(SoundManager.Instance.dogSFX.barkPoodle).length;
        else if (dog.DogData.id == 2) barkWaitTimer = SoundManager.Instance.PlaySound(SoundManager.Instance.dogSFX.barkPug).length;
        else if (dog.DogData.id == 3) barkWaitTimer = SoundManager.Instance.PlaySound(SoundManager.Instance.dogSFX.barkRetreiver).length;

        OnBark?.Invoke();
    }
}
