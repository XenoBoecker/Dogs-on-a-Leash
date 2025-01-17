using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{

    [SerializeField] TMP_Text objectiveScoreText, timeLeftText, totalScoreText;

    int objectiveScore;
    int timeLeft;

    int totalScore;

    // Start is called before the first frame update
    void Start()
    {
        objectiveScore = PlayerPrefs.GetInt("Score");
        timeLeft = PlayerPrefs.GetInt("TimeLeft");

        totalScore = CalculateTotalScore();

        UpdateUI();
    }

    private int CalculateTotalScore()
    {
        return objectiveScore * timeLeft;
    }

    // Update is called once per frame
    void UpdateUI()
    {
        objectiveScoreText.text = objectiveScore.ToString();
        timeLeftText.text = timeLeft.ToString();

        totalScoreText.text = totalScore.ToString();
    }
}
