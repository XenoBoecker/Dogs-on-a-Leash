using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    HumanMovement human;

    float goalDist;

    bool done;


    [SerializeField] float respawnActivateDist = 30, respawnForwardDist = 30, minRestPathLength = 50;

    [SerializeField] int maxRespawnCount = 2;


    [SerializeField] float mapWidth = 10;
    int respawnCount;

    // Start is called before the first frame update
    void Start()
    {
        human = FindObjectOfType<HumanMovement>();

        goalDist = FindObjectOfType<MapManager>().TotalPathLength;
    }

    // Update is called once per frame
    void Update()
    {
        if (done) return;

        if(human.transform.position.x - transform.position.x > respawnActivateDist)
        {
            if(human.transform.position.x + respawnForwardDist + minRestPathLength < goalDist)
            {
                if(respawnCount < maxRespawnCount)
                {
                    RespawnAt(human.transform.position.x + respawnForwardDist);
                }
            }
        }

        if (transform.position.x > goalDist)
        {
            done = true;
            FindObjectOfType<ScoreManager>().AddScore(1); // Explosion
        }
    }
    private void RespawnAt(float xPos)
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;

        float yPos = 1f; // Default y-position
        float zPos = UnityEngine.Random.Range(-mapWidth / 2, mapWidth / 2); // Random initial zPos
        bool positionFound = false;

        int maxAttempts = 10; // Avoid infinite loops
        int attempts = 0;

        while (!positionFound && attempts < maxAttempts)
        {
            Vector3 checkPosition = new Vector3(xPos, yPos + 1f, zPos); // Start the ray slightly above
            RaycastHit hit;

            if (Physics.Raycast(checkPosition, Vector3.down, out hit, Mathf.Infinity))
            {
                // If we hit an obstacle, pick a new random z position
                if (hit.collider.GetComponent<Obstacle>() != null)
                {
                    zPos = UnityEngine.Random.Range(-mapWidth / 2, mapWidth / 2);
                    attempts++;
                    continue;
                }
            }

            // If we reach here, the spot is free
            positionFound = true;
        }

        transform.position = new Vector3(xPos, yPos, zPos);
        respawnCount++;
    }
}
