using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDogController : DogController
{
    PlayerInput playerInput;

    public event Action OnInteract;
    public event Action OnStopInteract;

    private void OnEnable()
    {
        // Ensure PlayerInput component is enabled
        playerInput = GetComponentInParent<PlayerInput>();
        if (playerInput)
        {
            playerInput.onActionTriggered += OnActionTriggered;
        }
    }

    private void OnDisable()
    {
        // Unsubscribe from the PlayerInput events
        playerInput = GetComponentInParent<PlayerInput>();
        if (playerInput)
        {
            playerInput.onActionTriggered -= OnActionTriggered;
        }
    }

    private void OnActionTriggered(InputAction.CallbackContext context)
    {
        if (context.action.name == "Move")
        {
            if (context.phase == InputActionPhase.Performed)
            {
                movementInput = context.ReadValue<Vector2>();
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                movementInput = Vector2.zero;
            }
        }
        else if (context.action.name == "Zoomie")
        {
            if (context.phase == InputActionPhase.Performed)
            {
                ZoomieStart();
            }
        }
        else if(context.action.name == "Interact")
        {
            if (context.phase == InputActionPhase.Performed)
            {
                OnInteract?.Invoke();
            }
            else if(context.phase == InputActionPhase.Canceled)
            {
                OnStopInteract?.Invoke();
            }
        }
    }

    public void SetPlayerInput(PlayerInput input)
    {
        if (playerInput) playerInput.onActionTriggered -= OnActionTriggered;

        playerInput = input;

        playerInput.onActionTriggered += OnActionTriggered;
    }

    internal void StopMovement()
    {
        // rb.velocity = Vector2.zero;
        
        this.enabled = false;
    }

    internal void StartMovement()
    {
        this.enabled = true;
    }
}
