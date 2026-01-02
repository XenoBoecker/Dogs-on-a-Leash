using UnityEngine;

public static class Utils
{
    public static Vector3 V2_To_V3(Vector2 v)
    {
        return new Vector3(v.x, v.y, 0);
    }

    public static Vector3 GetPositionOutOfScreen(float margin)
    {
        Vector3 pos = Vector3.zero;

        float rand = Random.value;

        if (rand < 0.25f)
        {
            pos.x = -Camera.main.orthographicSize * Camera.main.aspect - margin;
            pos.y = Random.Range(-Camera.main.orthographicSize, Camera.main.orthographicSize);
        }
        else if (rand < 0.5f)
        {
            pos.x = Camera.main.orthographicSize * Camera.main.aspect + margin;
            pos.y = Random.Range(-Camera.main.orthographicSize, Camera.main.orthographicSize);
        }
        else if (rand < 0.75f)
        {
            pos.y = -Camera.main.orthographicSize - margin;
            pos.x = Random.Range(-Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize * Camera.main.aspect);
        }
        else
        {
            pos.y = Camera.main.orthographicSize + margin;
            pos.x = Random.Range(-Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize * Camera.main.aspect);
        }

        return pos;
    }

    public static Vector3 RotateVectorBy(float angle, Vector3 vector)
    {
        return Quaternion.Euler(0, 0, angle) * vector;
    }

    public static Vector3 GetRandomPosOnWalkableArea()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
            return Vector3.zero; // Return a default position if the main camera is not found
        }

        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        float spawnX = UnityEngine.Random.Range(-cameraWidth / 2f, cameraWidth / 2f);
        float spawnY = UnityEngine.Random.Range(-cameraHeight / 2f, cameraHeight / 2f);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);

        // Check for 2D colliders at the spawn position
        Collider2D[] colliders = Physics2D.OverlapPointAll(spawnPosition);

        // Check if any colliders were found
        if (colliders.Length > 0)
        {
            // If colliders are found, recursively call the method to find a new spawn position
            return GetRandomPosOnWalkableArea();
        }

        // If no colliders are found at the spawn position, return it
        return spawnPosition;
    }

    internal static Vector3 GetWalkablePosNextTo(Vector3 position, float maxDist)
    {
        Vector3 spawnPosition = position + Random.insideUnitSphere * maxDist;
        spawnPosition.z = 0f;

        // Check for 2D colliders at the spawn position
        Collider2D[] colliders = Physics2D.OverlapPointAll(spawnPosition);

        // Check if any colliders were found
        if (colliders.Length > 0)
        {
            // If colliders are found, recursively call the method to find a new spawn position
            return GetWalkablePosNextTo(position, maxDist);
        }

        // If no colliders are found at the spawn position, return it
        return spawnPosition;
    }

    public static bool OutOfShip(UnityEngine.Transform transformComponent)
    {
        float rayLength = 20f; // You can adjust the ray length as needed

        LayerMask wallLayer = LayerMask.GetMask("Wall");

        // Cast rays in all directions
        for (int i = 0; i < 360; i += 10) // Change the step size as needed
        {
            // Convert angle to radians
            float angle = i * Mathf.Deg2Rad;

            // Calculate direction vector based on angle
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            // Create a ray from the current position in the calculated direction
            Ray2D ray = new Ray2D(transformComponent.position, direction);

            // Perform the raycast
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, rayLength, wallLayer);

            if (hit.collider == null)
            {
                // If no wall is hit, return true
                return true;
            }
        }

        // If any ray hits a wall, return false
        return false;
    }
}