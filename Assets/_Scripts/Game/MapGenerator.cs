using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public List<GameObject> availableTiles; // List of available tile prefabs
    public int numberOfTiles = 10; // Number of tiles in the map
    public HumanMovement humanMovement; // Reference to the HumanMovement script

    private List<Vector3> occupiedPositions = new List<Vector3>(); // Track occupied positions

    enum TilePosition
    {
        Start,
        Middle,
        End
    }

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        List<GameObject> path = GeneratePath();

        PlaceTiles(path);
    }

    List<GameObject> GeneratePath()
    {
        List<GameObject> path = new List<GameObject>();
        Vector3 currentPosition = Vector3.zero;
        Vector3 currentDirection = Vector3.zero;
        occupiedPositions.Clear();

        for (int i = 0; i < numberOfTiles; i++)
        {
            GameObject chosenTile;
            if(i == 0) chosenTile = ChooseTile(currentPosition, currentDirection, TilePosition.Start);
            else if(i == numberOfTiles-1) chosenTile = ChooseTile(currentPosition, currentDirection, TilePosition.End);
            else chosenTile = ChooseTile(currentPosition, currentDirection, TilePosition.Middle);

            
            if (chosenTile == null)
            {
                Debug.LogWarning("Path generation failed. Regenerating...");
                return GeneratePath(); // Retry if the path fails
            }

            Tile tileScript = chosenTile.GetComponent<Tile>();
            if (tileScript == null)
            {
                Debug.LogError("Tile prefab is missing the Tile script.");
                return null;
            }

            if (occupiedPositions.Contains(currentPosition))
            {
                Debug.LogWarning("Path looped. Regenerating...");
                return GeneratePath();
            }

            path.Add(chosenTile);
            occupiedPositions.Add(currentPosition);

            Debug.Log("Add " + currentPosition);

            // Calculate the next position and direction
            currentPosition += tileScript.exitDirection;
            currentDirection = tileScript.exitDirection;
        }

        return path;
    }

    void PlaceTiles(List<GameObject> path)
    {
        Vector3 currentPosition = Vector3.zero;
        Vector3 currentDirection = Vector3.forward;

        foreach (GameObject tilePrefab in path)
        {
            GameObject newTile = Instantiate(tilePrefab, currentPosition, Quaternion.identity);
            Tile tileScript = newTile.GetComponent<Tile>();
            if (tileScript == null)
            {
                Debug.LogError("Tile prefab is missing the Tile script.");
                break;
            }

            // Rotate the tile to match the path direction
            Vector3 desiredDirection = currentDirection;
            float angle = Vector3.SignedAngle(tileScript.entryDirection, -desiredDirection, Vector3.up);
            newTile.transform.Rotate(Vector3.up, angle);

            // Adjust for rotation
            currentPosition += newTile.transform.TransformDirection(tileScript.exitDirection);
            currentDirection = newTile.transform.TransformDirection(tileScript.exitDirection);

            // Add waypoints to the HumanMovement script
            if (humanMovement != null)
            {
                humanMovement.AddWaypoints(tileScript.pathWaypoints);
            }
        }
    }

    GameObject ChooseTile(Vector3 position, Vector3 direction, TilePosition tilePosition)
    {
        List<GameObject> suitableTiles = new List<GameObject>();

        foreach (GameObject tilePrefab in availableTiles)
        {
            Tile tileScript = tilePrefab.GetComponent<Tile>();
            if (tileScript == null)
            {
                Debug.LogError("Tile prefab is missing the Tile script.");
                continue;
            }

            // Simulate the tile's rotation based on the current direction
            Vector3 adjustedEntryDirection = direction == Vector3.zero ? tileScript.entryDirection : -direction;
            float rotationAngle = Vector3.SignedAngle(tileScript.entryDirection, adjustedEntryDirection, Vector3.up);

            // Calculate the world-space exitDirection
            Vector3 rotatedExitDirection = Quaternion.Euler(0, rotationAngle, 0) * tileScript.exitDirection;
            Vector3 exitPosition = position + rotatedExitDirection;

            if (tilePosition == TilePosition.Start) // First tile (start position)
            {
                if (tileScript.entryDirection == Vector3.zero)
                {
                    suitableTiles.Add(tilePrefab);
                }
            }else if(tilePosition == TilePosition.End)
            {
                if (tileScript.exitDirection == Vector3.zero)
                {
                    suitableTiles.Add(tilePrefab);
                }
            }
            else
            {
                // Check if the exitPosition is not already occupied and avoid start tiles
                if (!occupiedPositions.Contains(exitPosition) && tileScript.entryDirection != Vector3.zero)
                {
                    suitableTiles.Add(tilePrefab);
                }
            }
        }

        if (suitableTiles.Count > 0)
        {
            return suitableTiles[Random.Range(0, suitableTiles.Count)];
        }

        return null;
    }

}