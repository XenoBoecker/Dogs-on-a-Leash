using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class LeaderboardManager : MonoBehaviour
{
    GameOver gameOverManager;


    [SerializeField] GameObject leaderboardPanel;


    [SerializeField] Transform playerScoreShowParent;

    [SerializeField] PlayerScoreShow playerScoreShowPrefab;

    List<PlayerScoreShow> playerScoreShowObjects = new List<PlayerScoreShow>();


    [SerializeField] TMP_Text showYourScoreAgain;
    [SerializeField] GameObject playAgainButton;
    [SerializeField] GameObject teamNameInputField;

    public Leaderboard leaderboard;
    public GameObject leaderBoardInputObject;
    public TMP_InputField playerNameInput;
    public Button submitScoreButton;
    public Button resetScoresButton;
    private int lastGameScore = 0; // Assume this is set when the game ends

    bool scoreSubmitted;

    private void Awake()
    {
        leaderboardPanel.SetActive(false);
    }

    void Start()
    {
        gameOverManager = FindObjectOfType<GameOver>();
        gameOverManager.OnShowLeaderboard += ShowLeaderboardCanvas;

        // Assuming the Leaderboard script is attached to a GameObject named "Leaderboard"
        leaderboard = GetComponent<Leaderboard>();
        leaderboard.LoadLeaderboard();
        leaderboard.OnPlayerScoresChanged += UpdateLeaderboardUI;
        UpdateLeaderboardUI();

        submitScoreButton.onClick.AddListener(OnSubmitScore);
        if (resetScoresButton != null) resetScoresButton.onClick.AddListener(ResetScores);

        GameOver(PlayerPrefs.GetInt("FinalScore"));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnSubmitScore();
        }
    }

    private void ShowLeaderboardCanvas()
    {
        leaderboardPanel.SetActive(true);
        if (PlayerPrefs.GetInt("OnlyShowLeaderboard") == 1)
        {
            leaderBoardInputObject.SetActive(false);
            submitScoreButton.gameObject.SetActive(false);
        }
        EventSystem.current.SetSelectedGameObject(teamNameInputField);
    }

    public void GameOver(int score)
    {
        showYourScoreAgain.text = score.ToString();
        lastGameScore = score;
        leaderBoardInputObject.SetActive(true);
        submitScoreButton.gameObject.SetActive(true);
    }

    public void OnSubmitScore()
    {
        if (scoreSubmitted) return;

        string playerName = playerNameInput.text;
        if (!string.IsNullOrEmpty(playerName))
        {
            scoreSubmitted = true;

            leaderboard.AddPlayer(playerName, lastGameScore);
            leaderBoardInputObject.SetActive(false);
            submitScoreButton.gameObject.SetActive(false);

            EventSystem.current.SetSelectedGameObject(playAgainButton);
        }
    }

    public void ResetScores()
    {
        leaderboard.ResetLeaderboard();
    }

    public void UpdateLeaderboardUI()
    {
        List<Leaderboard.Player> currentLeaderboard = leaderboard.GetPlayerScores();

        foreach (PlayerScoreShow playerScoreShow in playerScoreShowObjects)
        {
            Destroy(playerScoreShow.gameObject);
        }

        playerScoreShowObjects.Clear();
        for (int i = 0; i < currentLeaderboard.Count; i++)
        {
            PlayerScoreShow playerScoreShow = Instantiate(playerScoreShowPrefab, playerScoreShowParent);
            playerScoreShow.SetPlayer(currentLeaderboard[i], i + 1);
            playerScoreShowObjects.Add(playerScoreShow);
        }
    }

    void OnApplicationQuit()
    {
        // Save the leaderboard when the application quits
        leaderboard.SaveLeaderboard();
    }
}
