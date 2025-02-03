using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    HumanMovement human;

    float goalDist;

    bool done;


    [SerializeField] float respawnActivateDist = 30, respawnForwardDist = 30, minRestPathLength = 50;

    [SerializeField] int maxRespawnCount;
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
            Debug.Log("RespawnDist");
            if(human.transform.position.x + respawnForwardDist + minRestPathLength < goalDist)
            {
                Debug.Log("still enough space");
                if(respawnCount < maxRespawnCount)
                {
                    Debug.Log("respawn");
                    GetComponent<Rigidbody>().velocity = Vector3.zero;
                    transform.position = new Vector3(human.transform.position.x + respawnForwardDist, 1f, 0f);

                    respawnCount++;
                }
            }
        }

        if (transform.position.x > goalDist)
        {
            done = true;
            FindObjectOfType<ScoreManager>().AddScore(1); // Explosion
        }
    }
}
