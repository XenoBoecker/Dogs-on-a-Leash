using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector3 entryDirection; // Direction where the path enters the tile
    public Vector3 exitDirection; // Direction where the path exits the tile
    public List<Transform> pathWaypoints; // Waypoints defining the path on this tile
}
