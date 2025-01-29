using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKSpineResolver : MonoBehaviour
{
    [SerializeField]
    float leaningAngle = 30;
    [SerializeField]
    float velocityThreshold = 0.1f; // threshold below which no rotation is applied
    [SerializeField]
    float maxVelocity = 10;
    [SerializeField]
    float pullDirectionThreshold = 0.01f; // lowered threshold below which no rotation is applied based on leash pull direction
    [SerializeField]
    float pullMagnitudeScale = 10f; // scale factor to make pull magnitude more significant
    Rigidbody rb;
    IKHandFollowLeash iKHandFollowLeash;

    // Start is called before the first frame update
    void Start()
    {
        rb = GameObject.Find("Human").GetComponent<Rigidbody>();
        iKHandFollowLeash = GameObject.Find("Human").GetComponentInChildren<IKHandFollowLeash>();
    }

    // Update is called once per frame
    void Update()
    {
        // calculate the target angle based on the leash pull direction
        Vector3 pullDirection = iKHandFollowLeash.GetLeashPullDirection();
        float pullMagnitude = pullDirection.magnitude * pullMagnitudeScale; // scale the pull magnitude

        // if pull direction is below the threshold and velocity is below the threshold, do not apply any rotation
        if (pullMagnitude < pullDirectionThreshold && rb.velocity.magnitude < velocityThreshold)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, Time.deltaTime * 5);
            return;
        }

        Debug.Log("pullMagnitude: " + pullMagnitude);

        float targetAngle = 0;

        // if pull direction is above the threshold, calculate the target angle based on the pull direction
        if (pullMagnitude >= pullDirectionThreshold)
        {
            targetAngle = Mathf.Clamp(pullMagnitude, 0, 1) * leaningAngle;
        }
        // if velocity is above the threshold, calculate the target angle based on the velocity
        else if (rb.velocity.magnitude >= velocityThreshold)
        {
            float normalizedVelocity = Mathf.Clamp01(rb.velocity.magnitude / maxVelocity);
            targetAngle = normalizedVelocity * leaningAngle;
        }

        Quaternion targetRotation = Quaternion.Euler(targetAngle, 0, 0);

        // smoothly interpolate the current rotation towards the target rotation
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * 5);
    }
}