using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDogController : DogController
{
    PlayerInput playerInput;
    
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
    }

    public void SetPlayerInput(PlayerInput input)
    {
        if (playerInput) playerInput.onActionTriggered -= OnActionTriggered;

        playerInput = input;

        playerInput.onActionTriggered += OnActionTriggered;
    }
}
