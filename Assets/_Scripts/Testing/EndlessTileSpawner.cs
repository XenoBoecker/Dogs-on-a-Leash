using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTileSpawner : MapGenerator
{
    HumanMovement human;

    // Start is called before the first frame update
    protected override void Start()
    {
        human = FindObjectOfType<HumanMovement>();

        PlaceStartTile();
    }

    // Update is called once per frame
    void Update()
    {
        if (human.CurrentWaypointCount < 5)
        {
            PlaceNextTile();
        }
    }
}
