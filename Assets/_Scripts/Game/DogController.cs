using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DogController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float maxSpeed = 5f; // Maximum speed of the dog
    public AnimationCurve accelerationCurve; // Define acceleration based on time
    public float accelerationTime = 2f; // Time to reach max speed

    [Header("References")]
    public Rigidbody dogRigidbody; // Reference to the Rigidbody
    public Animator dogAnimator; // Optional animator for animations

    private Vector2 movementInput; // Current input values
    private float currentSpeed; // Current speed
    private float accelerationTimer; // Timer for animation curve


    // Ability variables
    public event Action OnZoomieStart;
    public float speedMultiplier = 1;

    private void OnEnable()
    {
        // Ensure PlayerInput component is enabled
        var playerInput = GetComponent<PlayerInput>();
        if (playerInput)
        {
            playerInput.onActionTriggered += OnActionTriggered;
        }
    }

    private void OnDisable()
    {
        // Unsubscribe from the PlayerInput events
        var playerInput = GetComponent<PlayerInput>();
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
                OnZoomieStart?.Invoke();
            }
        }
    }

    private void FixedUpdate()
    {
        MoveDog();
    }

    private void MoveDog()
    {
        // Calculate the input magnitude
        float inputMagnitude = movementInput.magnitude;

        if (inputMagnitude > 0.1f) // If there's movement input
        {
            // Increment acceleration timer and clamp to the duration
            accelerationTimer += Time.fixedDeltaTime / accelerationTime;
            accelerationTimer = Mathf.Clamp01(accelerationTimer);

            // Get the current speed from the animation curve
            currentSpeed = accelerationCurve.Evaluate(accelerationTimer) * maxSpeed * speedMultiplier;

            // Calculate movement direction
            Vector3 direction = new Vector3(movementInput.x, 0f, movementInput.y).normalized;

            // Apply velocity to the Rigidbody
            dogRigidbody.velocity = direction * currentSpeed + new Vector3(0f, dogRigidbody.velocity.y, 0f);

            // Rotate the dog towards movement direction
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                dogRigidbody.rotation = Quaternion.Slerp(dogRigidbody.rotation, targetRotation, Time.fixedDeltaTime * 10f);
            }

            // Set animator parameters if applicable
            if (dogAnimator)
            {
                dogAnimator.SetFloat("Speed", currentSpeed / maxSpeed);
            }
        }
        else
        {
            // Reset acceleration timer and reduce speed when input stops
            accelerationTimer = 0f;
            currentSpeed = 0f;
            dogRigidbody.velocity = new Vector3(0f, dogRigidbody.velocity.y, 0f);

            // Update animator
            if (dogAnimator)
            {
                dogAnimator.SetFloat("Speed", 0f);
            }
        }
    }
}
