using System;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int tileLength = 30; // Length of the tile

    internal void Setup()
    {
        GetComponent<SpawnObjectives>().SpawnAllObjectives();
    }
}
