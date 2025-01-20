using UnityEngine;

public class IKHandFollowLeash : MonoBehaviour
{
    [SerializeField] Transform human;
    [SerializeField] Transform shoulderAnchorPoint;

    [SerializeField] float handMinDistFromShoulder;
    [SerializeField] float handMaxDistFromShoulder;


    [SerializeField] float heightElbowMaxDiff;
    [SerializeField] float elbowMaxDist;

    [SerializeField] float rotationSpeed;
    [SerializeField] Vector3 testDir;
    [SerializeField] bool turnHuman;


    [SerializeField] float pullStrengthForMaxAuslenkung = 10;

    [SerializeField] float testPullStrength;
    [SerializeField] Vector3 handForwardVector = Vector3.up; // Adjust this if the hand's forward is different

    private void Update()
    {
        Vector3 dir = CalculateLeashPullingDirection();

        if (turnHuman) TurnHumanTowardsPullDirection(dir);

        float auslenkPercentage = Mathf.Clamp01(CalculateLeashPullStrength() / pullStrengthForMaxAuslenkung);
        // Update hand position
        transform.position = shoulderAnchorPoint.position + dir * (handMinDistFromShoulder + ((handMaxDistFromShoulder- handMinDistFromShoulder) * auslenkPercentage));

        Vector3 elbowDir = Vector3.Cross(Vector3.up, dir);
        transform.position += (1 - auslenkPercentage) * elbowMaxDist * elbowDir;
        transform.position += (1 - auslenkPercentage) * heightElbowMaxDiff * Vector3.down;

        // Constrain rotation to prevent flipping
        transform.rotation = ConstrainRotationWithFixedZ(Quaternion.LookRotation(dir, Vector3.up) * Quaternion.FromToRotation(handForwardVector, Vector3.forward));
    }
    private Quaternion ConstrainRotationWithFixedZ(Quaternion rotation)
    {
        // Get the forward and up vectors from the rotation
        Vector3 forward = rotation * Vector3.forward;
        Vector3 up = rotation * Vector3.up;

        // Recalculate the rotation with Z fixed to -90 degrees
        Quaternion fixedRotation = Quaternion.LookRotation(forward, up);
        fixedRotation *= Quaternion.Euler(0, 0, -90); // Apply -90 degrees to Z rotation

        return fixedRotation;
    }

    private void TurnHumanTowardsPullDirection(Vector3 dir)
    {
        // Calculate the target rotation based on the pull direction
        Quaternion targetRotation = Quaternion.LookRotation(dir, Vector3.up);

        // Smoothly interpolate the human's current rotation to the target rotation
        human.transform.rotation = Quaternion.Lerp(
            human.transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );
    }

    float CalculateLeashPullStrength()
    {
        return testPullStrength;
    }

    Vector3 CalculateLeashPullingDirection()
    {
        return testDir.normalized;
    }

    private Quaternion ConstrainRotationToUp(Quaternion rotation)
    {
        // Decompose the rotation into forward and up vectors
        Vector3 forward = rotation * Vector3.forward;
        forward.y = 0; // Remove any vertical tilt to keep the rotation flat
        forward.Normalize(); // Normalize the forward vector

        return Quaternion.LookRotation(forward, Vector3.up); // Reconstruct rotation without flipping
    }
}
