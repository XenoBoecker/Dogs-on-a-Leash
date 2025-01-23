using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{

    [SerializeField] GameObject timesUpPanel;

    [SerializeField] TMP_Text objectiveScoreText, timeLeftText, finalScoreText;

    int objectiveScore;
    int timeLeft;

    int finalScore;

    public event Action OnShowLeaderboard;

    // Start is called before the first frame update
    void Awake()
    {
        objectiveScore = PlayerPrefs.GetInt("Score");
        timeLeft = PlayerPrefs.GetInt("TimeLeft");

        finalScore = CalculateTotalScore();

        Debug.Log("Final score: " + finalScore);

        PlayerPrefs.SetInt("FinalScore", finalScore);

        UpdateUI();
    }

    private int CalculateTotalScore()
    {
        return objectiveScore * timeLeft;
    }

    // Update is called once per frame
    void UpdateUI()
    {
        if (timeLeft == 0) timesUpPanel.SetActive(true);
        else timesUpPanel.SetActive(false);

        objectiveScoreText.text = "Score: " + objectiveScore.ToString();
        timeLeftText.text = "Time: " + timeLeft.ToString();

        finalScoreText.text = "Final: " + finalScore.ToString();
    }

    public void ShowLeaderboard()
    {
        OnShowLeaderboard?.Invoke();
    }
}
