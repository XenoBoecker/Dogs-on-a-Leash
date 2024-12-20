using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DogController : MonoBehaviour
{
    public AnimationCurve accelerationCurve; // Controls speed increase
    public AnimationCurve decelerationCurve; // Controls speed decrease
    public float maxSpeed = 10f; // Maximum speed
    public float speedMultiplier = 1f;
    public float turnSpeed = 5f; // How quickly the dog rotates
    public float accelerationTime = 1f; // Time to reach max speed
    public float decelerationTime = 1f; // Time to stop from max speed

    float finalSpeed;

    protected Vector2 movementInput; // Current input values (set externally)

    private Rigidbody rb;
    
    private float currentSpeed = 0f; // Current speed magnitude
    private float accelerationTimer = 0f; // Timer for acceleration curve
    private float decelerationTimer = 0f; // Timer for deceleration curve
    private Vector3 targetDirection;

    public event Action OnZoomieStart;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetDirection = transform.forward; // Initially face forward
    }

    void Update()
    {
        Vector3 inputDirection = new Vector3(movementInput.x, 0f, movementInput.y).normalized;

        if (inputDirection != Vector3.zero)
        {
            // Update target direction based on input
            targetDirection = inputDirection;

            // Reset deceleration and start acceleration
            decelerationTimer = 0f;
            accelerationTimer += Time.deltaTime;
            accelerationTimer = Mathf.Clamp(accelerationTimer, 0f, accelerationTime);

            // Get speed based on acceleration curve
            currentSpeed = accelerationCurve.Evaluate(accelerationTimer / accelerationTime) * maxSpeed;

            currentSpeed = Mathf.Max(currentSpeed, rb.velocity.magnitude);

            finalSpeed = currentSpeed;
        }
        else
        {
            // Reset acceleration and start deceleration
            accelerationTimer = 0f;
            decelerationTimer += Time.deltaTime;
            decelerationTimer = Mathf.Clamp(decelerationTimer, 0f, decelerationTime);

            // Get speed based on deceleration curve
            currentSpeed = decelerationCurve.Evaluate(decelerationTimer / decelerationTime) * finalSpeed;
        }
    }

    void FixedUpdate()
    {
        // Smoothly rotate towards the target direction
        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
        }

        // Move the dog forward based on current speed
        Vector3 forwardVelocity = rb.transform.forward * currentSpeed;
        rb.velocity = forwardVelocity;
    }
    protected void ZoomieStart()
    {
        OnZoomieStart?.Invoke();
    }
}
