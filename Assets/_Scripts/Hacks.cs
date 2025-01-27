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
        if (Input.GetKeyDown(KeyCode.L)) EndGameWell();
        if (Input.GetKeyDown(KeyCode.O)) scoreManager.HackSetTimeLeft(0); // lose game
        if (Input.GetKeyDown(KeyCode.P)) scoreManager.AddScore(Random.Range(1, 20) * 100);
        if (Input.GetKeyDown(KeyCode.I)) scoreManager.AddScore(-Random.Range(1, 20) * 100);

    }

    private void EndGameWell()
    {
        scoreManager.AddScore(Random.Range(1, 100) * 100);
        scoreManager.HackSetTimeLeft(7);
        scoreManager.HackEndGame();
    }
}
