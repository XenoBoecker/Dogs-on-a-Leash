using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocalLobbyManager : MonoBehaviour
{
    LobbyData lobbyData;

    [SerializeField] string gameSceneName;

    List<LocalPlayer> players = new List<LocalPlayer>();
    [SerializeField] LobbyDogSelector[] lobbyDogSelectors;

    int currentPlayerCount;
    int aiCount;

    private void Awake()
    {
        lobbyData = FindObjectOfType<LobbyData>();
    }

    private void Start()
    {
        foreach (LobbyDogSelector selector in lobbyDogSelectors)
        {
            selector.gameObject.SetActive(false);
        }
    }

    public void RegisterPlayer(LocalPlayer player)
    {
        Debug.Log("Register player");

        players.Add(player);
        
        player.SetLobbyDogSelector(lobbyDogSelectors[currentPlayerCount]);

        player.OnConfirmSelectionChanged += CheckAllPlayerConfirmed;

        lobbyDogSelectors[currentPlayerCount + aiCount].gameObject.SetActive(true);

        currentPlayerCount++;
    }

    private void CheckAllPlayerConfirmed()
    {
        foreach (LocalPlayer player in players)
        {
            if (!player.IsSelectionConfirmed) return;
        }

        StartGame();
    }

    private void StartGame()
    {
        PlayerPrefs.SetInt("AICount", aiCount);

        for (int i = 0; i < lobbyDogSelectors.Length; i++)
        {
            PlayerPrefs.SetInt("Dog_" + i, Array.IndexOf(lobbyData.AvailableDogs, lobbyDogSelectors[i].GetDogData()));
        }

        SceneManager.LoadScene(gameSceneName);
    }

    public void AddAI()
    {
        if (aiCount + players.Count >= lobbyDogSelectors.Length) return;

        lobbyDogSelectors[currentPlayerCount + aiCount].gameObject.SetActive(true);

        aiCount++;
    }

    public void RemoveAI()
    {
        if (aiCount <= 0) return;

        aiCount--;

        lobbyDogSelectors[currentPlayerCount + aiCount].gameObject.SetActive(false);
    }
}