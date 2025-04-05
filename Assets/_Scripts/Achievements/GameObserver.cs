using UnityEngine;

public class GameObserver : MonoBehaviour
{
    public static GameObserver Instance;
    HumanMovement human;
    MapManager mapManager;

    public int HumanBumpCount { get; private set; }

    public int PickupCount { get; private set; }
    public int DigCount { get; private set; }
    public int PickupBoneCount { get; private set; }

    public int BarkAtDuckCount { get; private set; }
    public int BarkCount { get; private set; }

    public int TotalCollectedPoints { get; private set; }
    public int ThisRoundsHighScore { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    void Start()
    {
        Dog[] allDogs = FindObjectsOfType<Dog>();
        human = FindAnyObjectByType<HumanMovement>();
        mapManager = FindAnyObjectByType<MapManager>();

        ScoreManager.Instance.OnGameOver += OnGameOver;

        foreach (Dog dog in allDogs)
        {
            dog.GetComponent<DogBarking>().OnBark += () => BarkCount++;
            ScoreManager.Instance.OnAddScore += (int score) => TotalCollectedPoints += score;
            ScoreManager.Instance.OnScoreChanged += (int score, bool positive) =>
            {
                if (positive && score > ThisRoundsHighScore) ThisRoundsHighScore = score;
            };
            Interactable.OnTaskCompleted += OnTaskCompleted;
        }

        for (int i = 0; i < 4; i++)
        {
            if(allDogs.Length <= i)
            {
                PlayerPrefs.SetInt("Dog_" + i + "_ID", -1);
            }
            else
            {
                Dog dog = allDogs[i];

                PlayerPrefs.SetInt("Dog_" + i + "_ID", dog.DogData.id);
            }
        }


        human.OnHitObstacle += (Obstacle obstacle) => HumanBumpCount++;
    }

    private void OnTaskCompleted(Interactable interactable)
    {
        if (interactable is RemoveAndGetPointsInteractable) DigCount++;
        else PickupBoneCount++;

        PickupCount++;


    }

    void OnGameOver()
    {
        PlayerPrefs.SetInt("LevelLength", mapManager.TotalPathLength);

        PlayerPrefs.SetInt("Score", ScoreManager.Instance.TotalPickupScore);
        PlayerPrefs.SetInt("TimeLeft", (int)ScoreManager.Instance.TimeLeft);
        PlayerPrefs.SetInt("Distance", (int)human.transform.position.x);

        PlayerPrefs.SetInt("PickupCount", PickupCount);
        PlayerPrefs.SetInt("DigCount", DigCount);
        PlayerPrefs.SetInt("AllTime_DigCount", PlayerPrefs.GetInt("AllTime_DigCount") + DigCount);
        PlayerPrefs.SetInt("PickupBoneCount", PickupBoneCount);

        PlayerPrefs.SetInt("BumpedCount", HumanBumpCount);
        PlayerPrefs.SetInt("AllTime_BumpedCount", PlayerPrefs.GetInt("AllTime_BumpedCount") + HumanBumpCount);

        PlayerPrefs.SetInt("BarkAtDuckCount", BarkAtDuckCount);
        PlayerPrefs.SetInt("AllTime_BarkAtDuckCount", PlayerPrefs.GetInt("AllTime_BarkAtDuckCount") + BarkAtDuckCount);
        PlayerPrefs.SetInt("BarkCount", BarkCount);
        PlayerPrefs.SetInt("AllTime_BarkCount", PlayerPrefs.GetInt("AllTime_BarkCount") + BarkCount);

        PlayerPrefs.SetInt("AllTime_GamesWon", PlayerPrefs.GetInt("AllTime_GamesWon") + 1);

        PlayerPrefs.SetInt("TotalCollectedPoints", TotalCollectedPoints);
        PlayerPrefs.SetInt("ThisRoundsHighScore", ThisRoundsHighScore);
    }
}