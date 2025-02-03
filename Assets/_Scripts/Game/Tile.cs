using System;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int tileLength = 30; // Length of the tile

    public bool IsStreetTile;
    internal void Setup()
    {
        SpawnObjectives so = GetComponent<SpawnObjectives>();
        if(so) so.SpawnAllObjectives();
    }
}
