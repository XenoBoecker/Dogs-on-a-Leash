using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hacks : MonoBehaviour
{
    HumanMovement human;
    ScoreManager scoreManager;

    // Start is called before the first frame update
    void Start()
    {
        human = FindObjectOfType<HumanMovement>();
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) human.transform.position = new Vector3(99999, 0, 0); // finish game
        if (Input.GetKeyDown(KeyCode.O)) scoreManager.HackSetTimeLeft(0); // lose game
        if (Input.GetKeyDown(KeyCode.P)) scoreManager.AddScore(Random.Range(1, 20) * 100);
    }
}
