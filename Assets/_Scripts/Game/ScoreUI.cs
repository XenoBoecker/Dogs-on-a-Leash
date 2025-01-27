﻿using System;
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


    [Header("StartGameAnimation")]

    [SerializeField] float waitBeforeAnimStart = 0.5f;
    [SerializeField] Transform start, center, end;
    [SerializeField] AnimationCurve positionCurve;
    [SerializeField] float animDuration;

    [SerializeField] Vector3 startScale;
    [SerializeField] AnimationCurve sizeCurve;
    [SerializeField] float sizeScale;

    [SerializeField] float centerWaitTime = 0.5f;

    [SerializeField] Color warningBlinkColor = Color.red;

    [SerializeField] float blinkSizeScale = 0.05f, warnBlinkSpeed = 2;



    private Coroutine popScoreCoroutine;

    Vector3 scoreTextStartScale;

    private void Start()
    {
        ScoreManager.Instance.OnScoreChanged += OnScoreChanged;

        scoreTextStartScale = scoreText.transform.localScale;

        scoreText.text = "0.pts";
        timeText.gameObject.SetActive(false);
        startScale = timeText.transform.localScale;
    }

    private void Update()
    {
        float timeLeft = ScoreManager.Instance.TimeLeft;
        int minutes = (int)(timeLeft / 60);
        int seconds = (int)(timeLeft % 60);
        timeText.text = "Time: " + minutes.ToString() + ":" + seconds.ToString("00");
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

    public IEnumerator TimerFlyInCorner()
    {
        timeText.gameObject.SetActive(true);
        timeText.transform.position = start.position;

        yield return new WaitForSecondsRealtime(waitBeforeAnimStart);

        for (float i = 0; i < animDuration; i += Time.unscaledDeltaTime)
        {
            timeText.transform.position = Vector3.Lerp(start.position, center.position, positionCurve.Evaluate(i / animDuration));
            timeText.transform.localScale = startScale * (1 + sizeCurve.Evaluate(i / animDuration) * sizeScale);

            yield return null;
        }
        timeText.transform.position = center.position;
        timeText.transform.localScale = startScale * (1 + sizeScale);

        Vector3 blinkStartScale = timeText.transform.localScale;
        for (float i = 0; i < centerWaitTime; i += Time.unscaledDeltaTime)
        {
            float sinSquare = Mathf.Pow(Mathf.Sin(i * 2 * Mathf.PI * warnBlinkSpeed),2);
            timeText.color = Color.Lerp(baseColor, warningBlinkColor, sinSquare);// alternate between baseColor and warningBlinkColor
            timeText.transform.localScale = blinkStartScale * (1 + sinSquare * blinkSizeScale);

            yield return null;
        }
        timeText.color = baseColor;
        timeText.transform.localScale = blinkStartScale;

        for (float i = 0; i < animDuration; i += Time.unscaledDeltaTime)
        {
            timeText.transform.position = Vector3.Lerp(center.position, end.position, positionCurve.Evaluate(i / animDuration));
            timeText.transform.localScale = startScale * (1 + sizeCurve.Evaluate(1 - i / animDuration) * sizeScale);

            yield return null;
        }
        timeText.transform.position = end.position;
        timeText.transform.localScale = startScale;

    }
}
