using System;
using System.Collections;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    ChangeScenes sceneChanger;

    HumanMovement human;
    MapManager mapManager;

    [SerializeField] string endGameSceneName = "GameOver";
    [SerializeField] string failedEndGameSceneName = "GameOverFailed";

    [SerializeField] int totalTime;

    [SerializeField] int warningTime = 10;

    [SerializeField] float warningSoundVolumeMultiplier = 0.2f;

    [SerializeField] GameObject finishText;

    [SerializeField] AnimationCurve finishTextPopCurve;

    [SerializeField] float finishTextPopDuration;

    [SerializeField]
    float waitForFinishTextVanishTime = 0.5f;
    [SerializeField] float finishTextVanishTime;
    Vector3 finishTexBaseScale;

    float timeLeft;
    public float TimeLeft => timeLeft;

    int totalPickupScore;
    public int TotalPickupScore => totalPickupScore;

    bool waitingForGameStart = true;
    bool closeToEnd;
    bool gameOver;


    public event Action<int> OnAddScore;
    public event Action<int, bool> OnScoreChanged;

    public event Action OnCloseToEnd;
    public event Action OnGameOver;



    private void Awake()
    {
        Instance = this;
        FindObjectOfType<CameraMovement>().OnFlyThroughFinished += StartTimer;
    }

    // Start is called before the first frame update
    void Start()
    {
        sceneChanger = FindObjectOfType<ChangeScenes>();

        human = FindObjectOfType<HumanMovement>();
        human.OnHitObstacle += SubtractObstaclePoints;

        mapManager = FindObjectOfType<MapManager>();
        mapManager.OnGameEnd += EndGame;

        timeLeft = totalTime;
        finishText.SetActive(false);
        finishTexBaseScale = finishText.transform.localScale;


        AddScore(0);
    }

    // Update is called once per frame
    void Update()
    {

        if (waitingForGameStart) return;
        if (gameOver) return;

        timeLeft -= Time.deltaTime;

        if (timeLeft <= warningTime && !closeToEnd)
        {
            closeToEnd = true;
            OnCloseToEnd?.Invoke();
            StartCoroutine(EndWarning());
        }

        if (timeLeft <= 0)
        {
            timeLeft = 0;
            EndGame();
        }
    }

    void StartTimer()
    {
        waitingForGameStart = false;
    }

    private void SubtractObstaclePoints(Obstacle obstacle)
    {
        AddScore(obstacle.scoreValue);
    }

    public void AddScore(int score)
    {
        OnAddScore?.Invoke(score);

        bool positive = score >= 0;

        totalPickupScore += score;

        if (totalPickupScore < 0) totalPickupScore = 0;

        OnScoreChanged?.Invoke(totalPickupScore, positive);
    }

    void EndGame()
    {
        if (gameOver) return;
        gameOver = true;

        OnGameOver?.Invoke();


        if ((int)timeLeft == 0)
        {
            StartCoroutine(EndGameFailedCoroutine());
        }
        else
        {
            StartCoroutine(EndGameSuccessCoroutine());
        }
    }
    IEnumerator EndGameFailedCoroutine()
    {
        Time.timeScale = 0;

        SoundManager.Instance.PlaySound(SoundManager.Instance.uiSFX.endOfGameLossWhistle);

        yield return StartCoroutine(FindObjectOfType<CameraMovement>().FlyToBusCoroutine());

        yield return StartCoroutine(FindObjectOfType<Bus>().BusDriveAwayCoroutine());

        Time.timeScale = 1;

        sceneChanger.LoadScene(failedEndGameSceneName);
    }

    IEnumerator EndGameSuccessCoroutine()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.uiSFX.endOfGameSuccessWhistle);

        StartCoroutine(PopFinishText());

        yield return StartCoroutine(FindObjectOfType<Bus>().BusLeavingCoroutine());

        sceneChanger.LoadScene(endGameSceneName);
    }

    IEnumerator PopFinishText()
    {
        finishText.SetActive(true);

        for (float i = 0; i < finishTextPopDuration; i+=Time.deltaTime)
        {
            finishText.transform.localScale = finishTexBaseScale * finishTextPopCurve.Evaluate(i / finishTextPopDuration);
            yield return null;
        }
        finishText.transform.localScale = finishTexBaseScale;

        yield return new WaitForSeconds(waitForFinishTextVanishTime);

        for (float i = 0; i < finishTextVanishTime; i+=Time.deltaTime)
        {
            finishText.transform.localScale = finishTexBaseScale * (1- i/ finishTextVanishTime);
            yield return null;
        }
        finishText.transform.localScale = Vector3.zero;
        finishText.SetActive(false);
    }

    IEnumerator EndWarning()
    {
        for (int i = 0; i < warningTime-1; i++)
        {
            if (gameOver) yield break;

            SoundManager.Instance.PlaySound(SoundManager.Instance.uiSFX.timeWarnTick, null, warningSoundVolumeMultiplier);

            yield return new WaitForSeconds(1);
        }
    }

    public void HackSetTimeLeft(float t)
    {
        timeLeft = t;
    }
    public void HackSetTotalScore(int score)
    {
        totalPickupScore = score;
    }

    public void HackEndGame()
    {
        EndGame();
    }
}
