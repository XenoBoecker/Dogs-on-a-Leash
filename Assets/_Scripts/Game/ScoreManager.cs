using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    ChangeScenes sceneChanger;

    HumanMovement human;
    MapManager mapManager;
    InteractableDetector detector;

    [SerializeField] string endGameSceneName = "GameOver";
    [SerializeField] string failedEndGameSceneName = "GameOverFailed";

    [SerializeField] TMP_Text timeText;

    [SerializeField] TMP_Text scoreText;

    [SerializeField] int totalTime;

    float timeLeft;

    int totalScore;

    bool waitingForGameStart = true;

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
        timeText.text = "Time: " + timeLeft.ToString("F2");

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
        totalScore += score;

        if (totalScore < 0) totalScore = 0;

        scoreText.text = "Score: " + totalScore;
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
}
