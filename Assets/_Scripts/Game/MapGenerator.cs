using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    HumanMovement humanMovement;

    [SerializeField] List<Tile> startTiles;
    [SerializeField] List<Tile> availableTiles; // List of available tile prefabs
    [SerializeField] List<Tile> endTiles;

    [SerializeField] int levelLength = 120;

    int currentPathLength;

    

    enum TilePosition
    {
        Start,
        Middle,
        End
    }

    private void Awake()
    {
        humanMovement = FindObjectOfType<HumanMovement>();
    }

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        currentPathLength = 0;

        PlaceStartTile();

        while(currentPathLength < levelLength) PlaceNextTile();

        PlaceEndTile();
    }

    private void PlaceStartTile()
    {
        PlaceTile(startTiles[Random.Range(0, startTiles.Count)]);
    }

    private void PlaceNextTile()
    {
        PlaceTile(availableTiles[Random.Range(0, startTiles.Count)]);
    }

    private void PlaceEndTile()
    {
        PlaceTile(endTiles[Random.Range(0, startTiles.Count)]);
    }

    void PlaceTile(Tile tile)
    {
        Tile newTile = Instantiate(tile, currentPathLength * Vector3.right, Quaternion.identity);
        currentPathLength += tile.tileLength;

        humanMovement.AddWaypoints(newTile.pathWaypoints);
    }
    
}