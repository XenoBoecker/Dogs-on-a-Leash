using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public bool lockYRotation = false; // If true, the object won't rotate around the Y-axis

    private void Update()
    {
        if (Camera.main != null)
        {
            Vector3 targetPosition = Camera.main.transform.position;

            if (lockYRotation)
            {
                targetPosition.x = transform.position.x; // Keep Y position fixed
            }

            transform.LookAt(targetPosition);
        }
    }
}
