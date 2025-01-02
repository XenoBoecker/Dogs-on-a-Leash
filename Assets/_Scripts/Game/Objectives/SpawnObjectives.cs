using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectives : MonoBehaviour
{
    [SerializeField] Objective objectivePrefab;
    [SerializeField] int objectivesPerDog = 10;
    [SerializeField] DogData[] allDogs;
    [SerializeField] Vector2 mapSize = new Vector2(50,50);
    [SerializeField] float minDistBetweenObjectives = 2f; // Minimum distance between objectives
    [SerializeField] float minDistToCenter = 5f; // Minimum distance to the center


    [SerializeField] Transform objectiveParent;

    // Start is called before the first frame update
    void Start()
    {
        SpawnAllObjectives();
    }

    void SpawnAllObjectives()
    {
        List<Vector3> placedPositions = new List<Vector3>();

        foreach (var dog in allDogs)
        {
            for (int i = 0; i < objectivesPerDog; i++)
            {
                Vector3 position = GenerateValidPosition(placedPositions);
                if (position != Vector3.zero)
                {
                    // Instantiate the objective
                    Objective objective = Instantiate(objectivePrefab, position + transform.position, Quaternion.identity);

                    objective.transform.SetParent(objectiveParent);

                    // Assign objective type to match the current dog
                    objective.SetObjectiveType(dog.objectiveType);

                    // Add the position to the placed list
                    placedPositions.Add(position);
                }
            }
        }
    }

    Vector3 GenerateValidPosition(List<Vector3> placedPositions)
    {
        const int maxAttempts = 100; // Limit the number of placement attempts to avoid infinite loops
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            // Generate a random position within map bounds
            Vector3 position = new Vector3(
                Random.Range(-mapSize.x / 2, mapSize.x / 2),
                0,
                Random.Range(-mapSize.y / 2, mapSize.y / 2)
            );

            // Check if the position is too close to the center
            if (position.magnitude < minDistToCenter)
                continue;

            // Check if the position is too close to existing objectives
            bool isTooClose = false;
            foreach (var placedPosition in placedPositions)
            {
                if (Vector3.Distance(position, placedPosition) < minDistBetweenObjectives)
                {
                    isTooClose = true;
                    break;
                }
            }

            if (!isTooClose)
                return position; // Valid position found
        }

        Debug.LogWarning("Failed to place an objective after max attempts.");
        return Vector3.zero; // Return a fallback position if no valid position is found
    }
}
