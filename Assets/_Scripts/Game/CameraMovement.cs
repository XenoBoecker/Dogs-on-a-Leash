using JetBrains.Annotations;
using Photon.Pun.Demo.PunBasics;
using Photon.Pun.Demo.SlotRacer;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Transform human;

    [SerializeField] Vector3 offset;

    [SerializeField] float lerpSpeed = 2f;


    [SerializeField] float flyThroughStartWaitTime = 1f;


    [SerializeField] Transform bus;
    [SerializeField] Transform[] busWheels;
    [SerializeField] AnimationCurve busPositionCurve;
    [SerializeField] Transform busStartPos, busEndPos;
    [SerializeField] float busArrivalDuration;


    [SerializeField] AnimationCurve flyCurve;
    [SerializeField] float flyThroughDuration = 5f;


    [SerializeField] NumberDisplay countdownDisplay;
    [SerializeField] int startCountdownDuration = 3;
    [SerializeField] AnimationCurve countdownPopCurve;
    [SerializeField] float countdownPopScale = 1.05f;
    Vector3 countdownStartScale;

    [SerializeField] AnimationCurve gameLostFlyToBusCurve;
    [SerializeField] float distancePerSecond = 10;



    [SerializeField] Transform shakeObject;
    [SerializeField] float shakeDuration;
    [SerializeField] float shakeScale = 5;
    [SerializeField] AnimationCurve shakeCurve;
    float shakeValue;

    bool inFlyThrough;
    bool cameraMovementDeactivated;

    public event Action OnFlyThroughFinished;

    private void Start()
    {
        human.GetComponent<HumanMovement>().OnHitObstacle += ScreenShake;

        countdownDisplay.gameObject.SetActive(false);
        countdownStartScale = countdownDisplay.transform.localScale;
    }

    private void LateUpdate()
    {
        if (!human) return;
        if (inFlyThrough) return;
        if (cameraMovementDeactivated) return;

        float lerpedX = Mathf.Lerp(transform.position.x + offset.x, human.position.x, Time.deltaTime * lerpSpeed);

        transform.position = new Vector3(lerpedX, transform.position.y, transform.position.z);
    }

    internal void Setup()
    {
        human = FindObjectOfType<HumanMovement>().transform;
        StartCoroutine(StartGameCoroutine());
    }

    private IEnumerator StartGameCoroutine()
    {
        inFlyThrough = true;

        int totalDist = FindObjectOfType<MapManager>().TotalPathLength;

        transform.position = new Vector3(totalDist, transform.position.y, transform.position.z);

        // wait for fade
        yield return new WaitForSeconds(flyThroughStartWaitTime);

        Time.timeScale = 0;

        // bus drives into screen
        yield return StartCoroutine(FindObjectOfType<Bus>().BusArrivingCoroutine());

        // timer is shown and flies to upper corner
        yield return StartCoroutine(FindObjectOfType<ScoreUI>().TimerFlyInCorner());

        yield return StartCoroutine(CameraFlyThrough(totalDist));
        


        StartCoroutine(GameStartCountdown());
    }


    private IEnumerator CameraFlyThrough(float totalDist)
    {
        float startTime = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup < startTime + flyThroughDuration)
        {
            float xPos = (1 - flyCurve.Evaluate((Time.realtimeSinceStartup - startTime) / flyThroughDuration)) * totalDist;

            transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
            yield return null;
        }

        Time.timeScale = 1;
        PauseMenu.Instance.PauseGame();

        while (Time.timeScale < 1)
        {
            yield return null;
        }
        Time.timeScale = 0;

    }

    private IEnumerator GameStartCountdown()
    {

        countdownDisplay.gameObject.SetActive(true);

        for (int i = 0; i < startCountdownDuration; i++)
        {
            int number = startCountdownDuration - i;

            countdownDisplay.SetNumber(number);

            SoundManager.Instance.PlaySound(SoundManager.Instance.uiSFX.countDownTick);

            StartCoroutine(PopCountdownNumber());

            yield return new WaitForSecondsRealtime(1);
        }



        EndFlyThrough();



        countdownDisplay.gameObject.SetActive(true);

        countdownDisplay.SetNumber(0);
        SoundManager.Instance.PlaySound(SoundManager.Instance.uiSFX.countDownWhistle);

        StartCoroutine(PopCountdownNumber());

        yield return new WaitForSecondsRealtime(1);
        countdownDisplay.gameObject.SetActive(false);
    }


    IEnumerator PopCountdownNumber()
    {
        for (float i = 0; i < 1; i+= Time.unscaledDeltaTime)
        {
            countdownDisplay.transform.localScale = countdownStartScale * countdownPopCurve.Evaluate(i) * countdownPopScale;
            yield return null;
        }
        countdownDisplay.transform.localScale = Vector3.zero;
    }

    void EndFlyThrough()
    {
        // StopAllCoroutines(); countdown problem

        transform.position = new Vector3(0, transform.position.y, transform.position.z);

        Time.timeScale = 1;

        inFlyThrough = false;

        countdownDisplay.gameObject.SetActive(false);

        OnFlyThroughFinished?.Invoke();
    }

    public IEnumerator FlyToBusCoroutine()
    {
        float startPos = transform.position.x;
        float endPos = FindObjectOfType<MapManager>().TotalPathLength;
        float dist = endPos - startPos;
        float flyDuration = dist / distancePerSecond;

        for (float i = 0; i < flyDuration; i+=Time.unscaledDeltaTime)
        {
            float xPos = startPos + gameLostFlyToBusCurve.Evaluate(i / flyDuration) * dist;

            transform.position = new Vector3(xPos, transform.position.y, transform.position.z);

            yield return null;
        }
        transform.position = new Vector3(endPos, transform.position.y, transform.position.z);

        cameraMovementDeactivated = true;
    }

    public void HackSkipFlythrough()
    {
        StopAllCoroutines();
        EndFlyThrough();
    }

    internal void ScreenShake(Obstacle obstacle)
    {
        Debug.Log("Shake");
        StartCoroutine(ScreenShakeCoroutine());
    }


    IEnumerator ScreenShakeCoroutine()
    {
        Vector3 startPos = shakeObject.transform.position;

        for (float i = 0; i < shakeDuration; i+=Time.deltaTime)
        {
            shakeValue = shakeCurve.Evaluate(i / shakeDuration) * shakeScale;

            shakeObject.transform.position = startPos + Vector3.left * shakeValue;
            yield return null;
        }
        shakeObject.transform.position = startPos;
    }
}
