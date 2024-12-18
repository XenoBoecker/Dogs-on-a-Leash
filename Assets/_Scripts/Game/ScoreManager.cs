using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    HumanMovement human;


    [SerializeField] string endGameSceneName = "GameOver";

    [SerializeField] TMP_Text timeText;

    [SerializeField] TMP_Text scoreText;

    ObjectiveCollector[] objectiveCollectors;

    int totalScore;

    float startTime;

    // Start is called before the first frame update
    void Start()
    {
        human = FindObjectOfType<HumanMovement>();
        human.OnEndGame += EndGame;
        
        objectiveCollectors = FindObjectsOfType<ObjectiveCollector>();

        foreach (ObjectiveCollector objectiveCollector in objectiveCollectors)
        {
            objectiveCollector.OnScoreChanged += AddScore;
        }

        startTime = Time.time;

        AddScore(0);
    }

    private void AddScore(int score)
    {
        totalScore += score;

        scoreText.text = "Score: " + totalScore;
    }

    // Update is called once per frame
    void Update()
    {
        timeText.text = "Time: " + (Time.time - startTime).ToString("F2");
    }

    void EndGame()
    {
        PlayerPrefs.SetInt("Score", totalScore);
        PlayerPrefs.SetFloat("Time", Time.time - startTime);

        SceneManager.LoadScene(endGameSceneName);
    }
}
