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
<<<<<<< Updated upstream
        playerInput = GetComponent<PlayerDogController>().GetPlayerInput();
        if (playerInput)
        {
            playerInput.onActionTriggered -= OnActionTriggered;
        }
    }

    private void OnActionTriggered(InputAction.CallbackContext context)
    {
        if (context.action.name == "Bark")
        {
            if (context.phase == InputActionPhase.Performed)
            {
                PlayRandomDogBark();
            }
        }
    }
=======
        GetComponent<PlayerDogController>().OnBark -= PlayRandomDogBark;
    }
>>>>>>> Stashed changes
    private void PlayRandomDogBark()
    {
        SoundManager.Instance.PlaySound(dogBarkSounds);
    }
}
