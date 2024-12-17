using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    [SerializeField] TMP_Text timeText;

    [SerializeField] TMP_Text scoreText;

    ObjectiveCollector[] objectiveCollectors;

    int totalScore;

    float startTime;

    // Start is called before the first frame update
    void Start()
    {
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
}
