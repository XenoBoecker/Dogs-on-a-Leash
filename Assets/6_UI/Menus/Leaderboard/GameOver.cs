using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static Unity.VisualScripting.Member;

public class GameOver : MonoBehaviour
{

    [SerializeField] GameObject scorePanel;

    [SerializeField] TMP_Text objectiveScoreText, timeLeftText, timeLeftScoreText, finalScoreText;

    [SerializeField] Transform plus, equals;

    [SerializeField] AnimationCurve popCurve;

    [SerializeField] float popDuration, popScale;


    [SerializeField] int scorePerSecondLeft = 500;


    [SerializeField] float calcDuration = 1f;

    [SerializeField] float waitBeforeStartingCalculation = 0.3f, waitTimeBetweenCalculations = 0.3f;


    [SerializeField] AudioSource audioSource;

    [SerializeField] float volumeMultiplier = 1;

    int objectiveScore;
    int timeLeft;

    int finalScore;

    private bool isCalculatingScores;
    private bool skipCalculation;

    public event Action OnShowLeaderboard;

    // Start is called before the first frame update
    void Awake()
    {

        objectiveScore = PlayerPrefs.GetInt("Score");
        timeLeft = PlayerPrefs.GetInt("TimeLeft");

        finalScore = CalculateTotalScore();

        PlayerPrefs.SetInt("FinalScore", finalScore);

        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        SoundManager.Instance.OnSoundReload += SoundReload;
        audioSource.clip = SoundManager.Instance.uiSFX.scoreCounting;
        SoundReload();
    }

    private void Start()
    {
        scorePanel.SetActive(true);

        StartCoroutine(ShowScorCalculation());
    }

    private IEnumerator ShowScorCalculation()
    {
        isCalculatingScores = true;

        objectiveScoreText.text = "0";
        timeLeftScoreText.text = "0";
        finalScoreText.text = "0";

        timeLeftText.text = "Time Pts. (" + timeLeft.ToString() + "s. left)";
        plus.transform.localScale = Vector3.zero;
        equals.transform.localScale = Vector3.zero;

        for (float i = 0; i < waitBeforeStartingCalculation; i+= Time.deltaTime)
        {
            if (skipCalculation)
            {
                break;
            }
            yield return null;
        }

        if (objectiveScore > 0)
        {
            audioSource.Play();

            for (float i = 0; i < calcDuration; i += Time.deltaTime)
            {
                if (skipCalculation)
                {
                    break;
                }
                int score = (int)(objectiveScore * i / calcDuration);

                if (score > objectiveScore) score = objectiveScore;

                objectiveScoreText.text = score.ToString();
                yield return null;
            }

            audioSource.Stop();
        }

        objectiveScoreText.text = objectiveScore.ToString();

        skipCalculation = false;








        for (float i = 0; i < waitTimeBetweenCalculations / 2; i += Time.deltaTime)
        {
            if (skipCalculation)
            {
                break;
            }
            yield return null;
        }

        // spawn plus poping

        SoundManager.Instance.PlaySound(SoundManager.Instance.uiSFX.popUp);

        for (float i = 0; i < popDuration; i += Time.deltaTime)
        {
            if (skipCalculation)
            {
                break;
            }
            plus.transform.localScale = Vector3.one * popCurve.Evaluate(i / popDuration) * popScale;

            yield return null;
        }

        plus.transform.localScale = Vector3.one * popCurve.Evaluate(1) * popScale;

        for (float i = 0; i < waitTimeBetweenCalculations / 2; i += Time.deltaTime)
        {
            if (skipCalculation)
            {
                break;
            }
            yield return null;
        }

        audioSource.Play();

        for (float i = 0; i < calcDuration; i += Time.deltaTime)
        {
            if (skipCalculation)
            {
                break;
            }
            int score = (int)(timeLeft * scorePerSecondLeft * i / calcDuration);
            timeLeftScoreText.text = score.ToString();

            yield return null;
        }

        audioSource.Stop();

        timeLeftScoreText.text = (timeLeft * scorePerSecondLeft).ToString();

        skipCalculation = false;








        for (float i = 0; i < waitTimeBetweenCalculations / 2; i += Time.deltaTime)
        {
            if (skipCalculation)
            {
                break;
            }
            yield return null;
        }

        SoundManager.Instance.PlaySound(SoundManager.Instance.uiSFX.popUp);

        // spawn = popping
        for (float i = 0; i < popDuration; i += Time.deltaTime)
        {
            if (skipCalculation)
            {
                break;
            }
            equals.transform.localScale = Vector3.one * popCurve.Evaluate(i / popDuration) * popScale;

            yield return null;
        }

        equals.transform.localScale = Vector3.one * popCurve.Evaluate(1) * popScale;

        for (float i = 0; i < waitTimeBetweenCalculations / 2; i += Time.deltaTime)
        {
            if (skipCalculation)
            {
                break;
            }
            yield return null;
        }

        audioSource.Play();

        //calculate final score
        for (float i = 0; i < calcDuration; i += Time.deltaTime)
        {
            if (skipCalculation)
            {
                break;
            }
            int score = (int)(finalScore * i / calcDuration);

            finalScoreText.text = score.ToString();

            yield return null;
        }

        audioSource.Stop();

        finalScoreText.text = finalScore.ToString();

        skipCalculation = false;
        isCalculatingScores = false;

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
        if (isCalculatingScores)
        {
            skipCalculation = true;
        }
        else
        {
            scorePanel.SetActive(false);

            OnShowLeaderboard?.Invoke();
        }
    }
    private void SoundReload()
    {
        audioSource.volume = SoundManager.Instance.SFXVolume * volumeMultiplier;
    }
}
