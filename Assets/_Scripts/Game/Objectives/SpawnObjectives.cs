using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectives : MonoBehaviour
{
    [SerializeField] GameObject[] objectivePrefabs;
    [SerializeField] int objectiveCount = 10;

    [SerializeField] Vector2 mapSize = new Vector2(50,50);
    [SerializeField] float minDistBetweenObjectives = 5f; // Minimum distance between objectives
    [SerializeField] float minDistToCenter = 5f; // Minimum distance to the center


    [SerializeField] Transform objectiveParent;

    public void SpawnAllObjectives()
    {
        List<Vector3> placedPositions = new List<Vector3>();

        for (int i = 0; i < objectiveCount; i++)
        {
            Vector3 position = GenerateValidPosition(placedPositions);
            if (position != Vector3.zero)
            {
                GameObject randomObjective = objectivePrefabs[Random.Range(0, objectivePrefabs.Length)];

                // Instantiate the objective
                GameObject objective;
                if (PhotonNetwork.IsConnected) objective = PhotonNetwork.Instantiate(randomObjective.name, position + transform.position, Quaternion.identity);
                else objective = Instantiate(randomObjective, position + transform.position, Quaternion.identity);

                objective.transform.SetParent(objectiveParent);

                // Assign objective type to match the current dog
                // objective.SetObjectiveType((ObjectiveType)Random.Range(0, (int)ObjectiveType.ObjectiveTypeCount));

                // Add the position to the placed list
                placedPositions.Add(position);
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
            if (position.z < minDistToCenter && position.z > -minDistToCenter)
            {
                continue;
            }

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
