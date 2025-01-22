using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;
using UnityEngine.InputSystem;

public class DogBarking : MonoBehaviour
{
    public string dogSoundsPath = "Sound/SoundEffects/DogSFX/dog1_bark";
    private AudioClip[] dogBarkSounds;

    PlayerInput playerInput;

    void Awake()
    {
        dogBarkSounds = Resources.LoadAll<AudioClip>(dogSoundsPath);
    }

    void OnEnable()
    {
        if(GetComponent<PlayerDogController>().GetPlayerInput() == null)
        {
            Invoke("SetPlayerInput", 0.1f);
        }
    }

    void SetPlayerInput()
    {
        if(GetComponent<PlayerDogController>().GetPlayerInput() == null)
        {
            Invoke("SetPlayerInput", 0.1f);
        }

        playerInput = GetComponent<PlayerDogController>().GetPlayerInput();
        if (playerInput)
        {
            playerInput.onActionTriggered += OnActionTriggered;
        }
    }

    private void OnDisable()
    {
        // Unsubscribe from the PlayerInput events
        playerInput = GetComponent<PlayerDogController>().GetPlayerInput();
        if (playerInput)
        {
            playerInput.onActionTriggered -= OnActionTriggered;
        }
    }

    private void OnActionTriggered(InputAction.CallbackContext context)
    {
        Debug.Log("Action Triggered: " + context.action.name);
        if (context.action.name == "Bark")
        {
            Debug.Log("Bark");
            if (context.phase == InputActionPhase.Performed)
            {
                Debug.Log("Bark Performed");
                PlayRandomDogBark();
            }
        }
    }
    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.M))
    //     {
    //         PlayRandomDogBark();
    //     }
    // }

    private void PlayRandomDogBark()
    {
        int randomIndex = UnityEngine.Random.Range(0, dogBarkSounds.Length);
        AudioClip selectedBark = dogBarkSounds[randomIndex];

        SoundManager.Instance.PlaySound(selectedBark);
    }
}
