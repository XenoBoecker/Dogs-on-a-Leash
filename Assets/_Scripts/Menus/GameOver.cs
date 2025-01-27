using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameOver : MonoBehaviour
{

    [SerializeField] GameObject scorePanel;

    [SerializeField] TMP_Text objectiveScoreText, timeLeftText, timeLeftScoreText, finalScoreText;


    [SerializeField] int scorePerSecondLeft = 500;


    [SerializeField] float calcDuration = 1f;

    [SerializeField] float waitBeforeStartingCalculation = 0.3f, waitTimeBetweenCalculations = 0.3f;

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
    }

    private void Start()
    {
        scorePanel.SetActive(true);

        StartCoroutine(ShowScorCalculation());
    }

    private IEnumerator ShowScorCalculation()
    {
        objectiveScoreText.text = "0";
        timeLeftScoreText.text = "0";
        finalScoreText.text = "0";

        timeLeftText.text = timeLeft.ToString();

        yield return new WaitForSeconds(waitBeforeStartingCalculation);

        for (float i = 0; i < calcDuration; i+=Time.deltaTime)
        {
            int score = (int)(objectiveScore * i / calcDuration);

            if (score > objectiveScore) score = objectiveScore;

            objectiveScoreText.text = score.ToString();
            finalScoreText.text = score.ToString();
            yield return null;
        }

        objectiveScoreText.text = objectiveScore.ToString();
        finalScoreText.text = objectiveScore.ToString();

        yield return new WaitForSeconds(waitTimeBetweenCalculations);

        for (int i = 1; i <= timeLeft; i++)
        {
            int score = (i * scorePerSecondLeft);
            timeLeftScoreText.text = score.ToString();

            finalScoreText.text = (objectiveScore + score).ToString();

            yield return new WaitForSeconds(calcDuration / timeLeft);
        }

        // UpdateUI();
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
