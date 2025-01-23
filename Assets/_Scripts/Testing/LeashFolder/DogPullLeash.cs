using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class DogPullLeash : MonoBehaviour
{
    PlayerInput playerInput;

    LeashManager myLeash;

    void Awake()
    {
        myLeash = GetComponent<LeashManager>();
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

    void OnActionTriggered(InputAction.CallbackContext context)
    {
        if (context.action.name == "PullLeash")
        {
            if (context.phase == InputActionPhase.Performed)
            {
                Debug.Log("Pulling Leash");
                myLeash.PullHuman();
            }
        }
    }
}
