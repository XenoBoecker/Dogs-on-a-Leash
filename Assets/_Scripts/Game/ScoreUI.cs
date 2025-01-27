using System;
using System.Collections;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] TMP_Text timeText;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] AnimationCurve popScoreCurve;
    [SerializeField] float popScoreDuration = 0.2f;
    [SerializeField] float popScoreScale = .05f;


    [SerializeField] Color baseColor, gainColor, lossColor;

    private Coroutine popScoreCoroutine;

    Vector3 scoreTextStartScale;

    private void Start()
    {
        ScoreManager.Instance.OnScoreChanged += OnScoreChanged;

        scoreTextStartScale = scoreText.transform.localScale;

        scoreText.text = "0.pts";
    }

    private void OnScoreChanged(int score, bool positive)
    {
        scoreText.text = score + ".pts";

        // Restart the pop effect from the current size
        if (popScoreCoroutine != null)
        {
            StopCoroutine(popScoreCoroutine);
        }
        popScoreCoroutine = StartCoroutine(PopScoreText(positive));
    }

    private void Update()
    {
        float timeLeft = ScoreManager.Instance.TimeLeft;
        int minutes = (int)(timeLeft / 60);
        int seconds = (int)(timeLeft % 60);
        timeText.text = "Time: " + minutes.ToString() + ":" + seconds.ToString();
    }

    private IEnumerator PopScoreText(bool positive)
    {
        float elapsedTime = 0f;

        Color targetColor = positive ? gainColor : lossColor; // Determine the target color based on the positive flag.
        Color initialColor = baseColor; // Start with the base color.

        while (elapsedTime < popScoreDuration)
        {
            float t = elapsedTime / popScoreDuration;

            // Evaluate the curve to get the current scale factor.
            float curveValue = popScoreCurve.Evaluate(t);

            // Lerp the color based on the curve value.
            scoreText.color = Color.Lerp(initialColor, targetColor, curveValue);

            // Scale the text according to the curve value.
            scoreText.rectTransform.localScale = scoreTextStartScale * (1 + (curveValue * popScoreScale));

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Reset to the original scale and base color at the end.
        scoreText.rectTransform.localScale = scoreTextStartScale;
        scoreText.color = baseColor;
    }

}
