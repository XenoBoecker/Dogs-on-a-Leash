using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    HumanMovement humanMovement;

    [SerializeField] List<Tile> startTiles;
    [SerializeField] List<Tile> availableTiles; // List of available tile prefabs

    [SerializeField] List<Tile> highDifficultyTiles;
    [SerializeField] List<Tile> endTiles;


    [SerializeField] Transform tileParent;

    public int levelLength = 120;

    int currentPathLength;
    public int TotalPathLength => currentPathLength - 15;

    List<int> tilesUsedIndices = new List<int>();
    bool highDifficultyActivated;

    public event Action OnGameEnd;

    enum TilePosition
    {
        Start,
        Middle,
        End
    }


    private void Update()
    {
        if (!humanMovement) return;

        if (humanMovement.transform.position.x >= TotalPathLength) EndGame();
    }

    private void EndGame()
    {
        OnGameEnd?.Invoke();
    }

    void GenerateMap()
    {
        UnityEngine.Random.InitState(PlayerPrefs.GetInt("Seed"));
        // Debug.Log("Seed: " + PlayerPrefs.GetInt("Seed"));

        currentPathLength = 0;

        PlaceStartTile();

        while (currentPathLength < levelLength)
        {
            PlaceNextTile();

            if(currentPathLength > levelLength/2 && !highDifficultyActivated)
            {
                highDifficultyActivated = true;
                for (int i = 0; i < highDifficultyTiles.Count; i++)
                {
                    availableTiles.Add(highDifficultyTiles[i]);
                }
            }
        }

        PlaceEndTile();
    }

    protected void PlaceStartTile()
    {
        PlaceTile(startTiles[UnityEngine.Random.Range(0, startTiles.Count)]);
    }

    protected void PlaceNextTile()
    {
        int index = UnityEngine.Random.Range(0, availableTiles.Count);
        int counter = 0;

        while(counter < 50 && tilesUsedIndices.Contains(index) && tilesUsedIndices.Count < availableTiles.Count)
        {
            counter++;

            index = UnityEngine.Random.Range(0, availableTiles.Count);
        }


        tilesUsedIndices.Add(index);

        PlaceTile(availableTiles[index]);
    }

    private void PlaceEndTile()
    {
        PlaceTile(endTiles[UnityEngine.Random.Range(0, endTiles.Count)]);
    }

    void PlaceTile(Tile tile)
    {
        Tile newTile;
        if (PhotonNetwork.IsConnected) newTile = PhotonNetwork.Instantiate(tile.name, currentPathLength * Vector3.right, Quaternion.identity).GetComponent<Tile>();
        else newTile = Instantiate(tile, currentPathLength * Vector3.right, Quaternion.identity).GetComponent<Tile>();

        newTile.transform.SetParent(tileParent);

        newTile.Setup();

        currentPathLength += tile.tileLength;
    }

    internal virtual void Setup()
    {
        humanMovement = FindObjectOfType<HumanMovement>();

        if (PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient) return;

        GenerateMap();
    }
}
