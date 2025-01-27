using System;
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

    float timeLeft;
    public float TimeLeft => timeLeft;

    int totalScore;

    bool waitingForGameStart = true;

    public event Action<int, bool> OnScoreChanged;

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

        AddScore(0);
    }

    // Update is called once per frame
    void Update()
    {

        if (waitingForGameStart) return;

        timeLeft -= Time.deltaTime;

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
        bool positive = score >= 0;

        totalScore += score;

        if (totalScore < 0) totalScore = 0;

        OnScoreChanged?.Invoke(totalScore, positive);
    }

    void EndGame()
    {
        PlayerPrefs.SetInt("Score", totalScore);
        PlayerPrefs.SetInt("TimeLeft", (int)timeLeft);
        PlayerPrefs.SetInt("Distance", (int)human.transform.position.x);
        PlayerPrefs.SetInt("PickupCount", InteractableDetector.PickupCount);
        PlayerPrefs.SetInt("BumpedCount", human.BumpedCount);
        PlayerPrefs.SetInt("LevelLength", mapManager.currentPathLength);

        if ((int)timeLeft == 0) sceneChanger.LoadScene(failedEndGameSceneName);
        else sceneChanger.LoadScene(endGameSceneName);
    }

    public void HackSetTimeLeft(float t)
    {
        timeLeft = t;
    }
    public void HackSetTotalScore(int score)
    {
        totalScore = score;
    }

    public void HackEndGame()
    {
        EndGame();
    }
}
