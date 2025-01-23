using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    HumanMovement humanMovement;

    [SerializeField] List<Tile> startTiles;
    [SerializeField] List<Tile> availableTiles; // List of available tile prefabs
    [SerializeField] List<Tile> endTiles;


    [SerializeField] Transform tileParent;

    public int levelLength = 120;

    public int currentPathLength;

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

        if (humanMovement.transform.position.x >= currentPathLength - 15) EndGame();
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

        while(currentPathLength < levelLength) PlaceNextTile();

        PlaceEndTile();
    }

    protected void PlaceStartTile()
    {
        PlaceTile(startTiles[UnityEngine.Random.Range(0, startTiles.Count)]);
    }

    protected void PlaceNextTile()
    {
        PlaceTile(availableTiles[UnityEngine.Random.Range(0, availableTiles.Count)]);
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
