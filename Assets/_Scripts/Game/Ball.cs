using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    float goalDist;

    bool done;
    // Start is called before the first frame update
    void Start()
    {
        goalDist = FindObjectOfType<MapManager>().TotalPathLength;
    }

    // Update is called once per frame
    void Update()
    {
        if (done) return;

        if (transform.position.x > goalDist)
        {
            done = true;
            FindObjectOfType<ScoreManager>().AddScore(1); // Explosion
        }
    }
}
