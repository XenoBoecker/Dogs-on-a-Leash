using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    HumanMovement human;
    MapManager mapManager;

    [SerializeField] string endGameSceneName = "GameOver";

    [SerializeField] TMP_Text timeText;

    [SerializeField] TMP_Text scoreText;

    int totalScore;

    float startTime;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        human = FindObjectOfType<HumanMovement>();
        human.OnHitObstacle += SubtractObstaclePoints;

        mapManager = FindObjectOfType<MapManager>();
        mapManager.OnGameEnd += EndGame;

        startTime = Time.time;

        AddScore(0);
    }

    private void SubtractObstaclePoints(Obstacle obstacle)
    {
        AddScore(obstacle.scoreValue);
    }

    public void AddScore(int score)
    {
        totalScore += score;

        if (totalScore < 0) totalScore = 0;

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
