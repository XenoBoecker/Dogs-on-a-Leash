using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{

    [SerializeField] GameObject scorePanel;

    [SerializeField] TMP_Text objectiveScoreText, timeLeftText, finalScoreText;


    [SerializeField] int scorePerSecondLeft = 500;

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

    private void Start()
    {
        scorePanel.SetActive(true);
    }

    private int CalculateTotalScore()
    {
        return objectiveScore + timeLeft * scorePerSecondLeft;
    }

    // Update is called once per frame
    void UpdateUI()
    {
        objectiveScoreText.text = objectiveScore.ToString();
        timeLeftText.text = timeLeft.ToString();

        finalScoreText.text = finalScore.ToString();
    }

    public void ShowLeaderboard()
    {
        scorePanel.SetActive(false);

        OnShowLeaderboard?.Invoke();
    }
}
