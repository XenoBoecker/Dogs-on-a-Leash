using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ArmFlailing : MonoBehaviour
{
    Rigidbody rb;
    Vector3 initialPosition;
    public float flailingSpeed = 1.0f;
    public float flailingRadius = 1.0f;
    public Transform nextBone;
    public float maxSpeed = 10.0f; // Maximum speed at which the maximum angle is reached
    public float maxAngle = 45.0f; // Maximum rotation angle in degrees
    public float rotationSpeed = 5.0f; // Speed of the slerp rotation

    // Start is called before the first frame update
    void Start()
    {
        rb = GameObject.Find("Human").GetComponent<Rigidbody>();
        initialPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (rb != null)
        {
            Vector3 flailDirection = rb.velocity;
            float flailAmount = flailDirection.magnitude * flailingSpeed;

            // Calculate the circular motion
            float time = Time.time * flailingSpeed;
            float x = Mathf.Cos(time) * flailingRadius;
            float y = Mathf.Sin(time) * flailingRadius;
            float z = Mathf.Sin(time) * flailingRadius;

            // Apply the circular motion to the game object
            transform.localPosition = initialPosition + new Vector3(x, y, z);

            // Apply the circular motion to the nextBone
            if (nextBone != null)
            {
                nextBone.localPosition = initialPosition + new Vector3(x, y, z);
            }

            // Calculate the target rotation angle based on the velocityMagnitude
            float velocity = rb.velocity.magnitude;
            float normalizedVelocity = Mathf.Clamp01(velocity / maxSpeed);

            float targetAngle = maxAngle * normalizedVelocity;

            // Calculate the target rotation
            Quaternion targetRotation = Quaternion.Euler(targetAngle, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);

            // Smoothly interpolate the rotation using Slerp
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}