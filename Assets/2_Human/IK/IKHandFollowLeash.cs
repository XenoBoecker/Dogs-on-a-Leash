
using UnityEngine;

public class IKHandFollowLeash : MonoBehaviour
{
    LeashManager[] dogLeashes;

    [SerializeField] Transform human;
    [SerializeField] Transform shoulderAnchorPoint;

    [SerializeField] float handMinDistFromShoulder;
    [SerializeField] float handMaxDistFromShoulder;


    [SerializeField] float heightElbowMaxDiff;
    [SerializeField] float elbowMinDist;
    [SerializeField] float elbowMaxDist;

    [SerializeField] float rotationSpeed;
    [SerializeField] bool turnHuman;


    [SerializeField] float pullStrengthForMaxAuslenkung = 10;

    [SerializeField] Vector3 pullDir;
    [SerializeField] float currentPullStrength;
    [SerializeField] Vector3 handForwardVector = Vector3.up; // Adjust this if the hand's forward is different


    [SerializeField] Vector3 baseForceVectorDir;

    private void Start()
    {
        dogLeashes = FindObjectsOfType<LeashManager>();
    }

    private void Update()
    {
        Vector3 leashCombinedForce = LeashedCombinedForce();

        pullDir = leashCombinedForce.normalized;
        currentPullStrength = leashCombinedForce.magnitude;

        if (turnHuman) TurnHumanTowardsPullDirection(pullDir);

        float auslenkPercentage = Mathf.Clamp01(currentPullStrength / pullStrengthForMaxAuslenkung);
        // Update hand position
        transform.position = shoulderAnchorPoint.position + pullDir * (handMinDistFromShoulder + ((handMaxDistFromShoulder- handMinDistFromShoulder) * auslenkPercentage));

        Vector3 elbowDir = Vector3.Cross(Vector3.up, pullDir);
        transform.position += (elbowMinDist + (1 - auslenkPercentage) * (elbowMaxDist- elbowMinDist)) * elbowDir;
        transform.position += (1 - auslenkPercentage) * heightElbowMaxDiff * Vector3.down;

        // Constrain rotation to prevent flipping
        transform.rotation = ConstrainRotationWithFixedZ(Quaternion.LookRotation(pullDir, Vector3.up) * Quaternion.FromToRotation(handForwardVector, Vector3.forward));
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

    Vector3 LeashedCombinedForce()
    {
        Vector3 result = Vector3.zero;

        for (int i = 0; i < dogLeashes.Length; i++)
        {
            result += dogLeashes[i].CurrentForceOnHuman();
        }

        result.y = 0;

        if (result == Vector3.zero) return baseForceVectorDir;

        return result;
    }

    public Vector3 GetLeashPullDirection() // Helper method for me :3
    {
        Vector3 result = Vector3.zero;

        for (int i = 0; i < dogLeashes.Length; i++)
        {
            result += dogLeashes[i].CurrentForceOnHuman();
        }

        result.y = 0;

        return result;
    }

    public Vector3 GetPullDir()
    {
        return pullDir;
    }
}
