using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTileSpawner : MapManager
{
    HumanMovement human;

    // Start is called before the first frame update
    protected void Start()
    {
        human = FindObjectOfType<HumanMovement>();

        PlaceStartTile();
    }

    // Update is called once per frame
    void Update()
    {
        if (TotalPathLength - human.transform.position.x < 50 || TotalPathLength - Camera.main.transform.position.x < 50)
        {
            PlaceNextTile();
        }
    }

    internal override void Setup()
    {
       
    }
}
