using Photon.Pun;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DogController : MonoBehaviour
{
    PhotonView view;

    public AnimationCurve accelerationCurve; // Controls speed increase
    public AnimationCurve decelerationCurve; // Controls speed decrease
    public float maxSpeed = 10f; // Maximum speed
    public float speedMultiplier = 1f;
    public float turnSpeed = 5f; // How quickly the dog rotates
    public float accelerationTime = 1f; // Time to reach max speed
    public float decelerationTime = 1f; // Time to stop from max speed

    float speedBeforeDecelleration;

    protected Vector2 movementInput; // Current input values (set externally)

    protected Rigidbody rb;
    
    private float currentSpeed = 0f; // Current speed magnitude
    private float accelerationTimer = 0f; // Timer for acceleration curve
    private float decelerationTimer = 0f; // Timer for deceleration curve
    private Vector3 targetDirection;

    public event Action OnZoomieStart;

    void Awake()
    {
        view = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        targetDirection = transform.forward; // Initially face forward

        if (PhotonNetwork.IsConnected && !view.IsMine) rb.isKinematic = true;
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

            speedBeforeDecelleration = currentSpeed;
        }
        else
        {
            // Reset acceleration and start deceleration
            accelerationTimer = 0f;
            decelerationTimer += Time.deltaTime;
            decelerationTimer = Mathf.Clamp(decelerationTimer, 0f, decelerationTime);

            // Get speed based on deceleration curve
            currentSpeed = decelerationCurve.Evaluate(decelerationTimer / decelerationTime) * speedBeforeDecelleration;
        }
    }

    // void FixedUpdate()
    // {

    //     if (PhotonNetwork.IsConnected && !view.IsMine) return;

    //     // Smoothly rotate towards the target direction
    //     if (targetDirection != Vector3.zero)
    //     {
    //         Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
    //         Quaternion rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
    //         transform.rotation = rotation;
            
    //         // Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
    //         // Quaternion rotation = Quaternion.Slerp(rb.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
    //         // rb.rotation = rotation;
    //     }

    //     Vector3 forwardVelocity = transform.forward * currentSpeed;

    //     if (forwardVelocity.magnitude > maxSpeed)
    //     {
    //         forwardVelocity = forwardVelocity.normalized * maxSpeed;
    //     }
        
    //     transform.Translate(forwardVelocity * Time.fixedDeltaTime, Space.World);

    //     // Move the dog forward based on current speed
    //     // Vector3 forwardVelocity = rb.transform.forward * currentSpeed;
    //     // rb.velocity = forwardVelocity;
    //     // 
    //     // if (rb.velocity.magnitude > maxSpeed)
    //     // {
    //     //     rb.velocity = rb.velocity.normalized * maxSpeed;
    //     // }
    // }

    void FixedUpdate()
    {
        if (PhotonNetwork.IsConnected && !view.IsMine) return;

        // Smoothly rotate towards the target direction
        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            Quaternion rotation = Quaternion.Slerp(rb.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(rotation);
        }

        Vector3 forwardVelocity = transform.forward * currentSpeed;

        if (forwardVelocity.magnitude > maxSpeed)
        {
            forwardVelocity = forwardVelocity.normalized * maxSpeed;
        }

        rb.MovePosition(rb.position + forwardVelocity * Time.fixedDeltaTime);
    }
    protected void ZoomieStart()
    {
        OnZoomieStart?.Invoke();
    }
}
