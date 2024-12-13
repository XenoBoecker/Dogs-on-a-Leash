﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{

    [SerializeField] string gameSceneName;

    List<LobbyPlayer> players = new List<LobbyPlayer>();
    [SerializeField] LobbyDogSelector[] lobbyDogSelectors;

    int currentPlayerCount;
    int aiCount;

    private void Start()
    {
        foreach (LobbyDogSelector selector in lobbyDogSelectors)
        {
            selector.gameObject.SetActive(false);
        }
    }

    public void RegisterPlayer(LobbyPlayer player)
    {
        Debug.Log("Register player");

        players.Add(player);

        if (player.PlayerName == "") player.SetPlayerName("P" + (currentPlayerCount+1));
        
        player.SetLobbyDogSelector(lobbyDogSelectors[currentPlayerCount]);

        player.OnConfirmSelectionChanged += CheckAllPlayerConfirmed;

        lobbyDogSelectors[currentPlayerCount + aiCount].gameObject.SetActive(true);

        currentPlayerCount++;
    }

    private void CheckAllPlayerConfirmed()
    {
        foreach (LobbyPlayer player in players)
        {
            if (!player.IsSelectionConfirmed) return;
        }

        StartGame();
    }

    private void StartGame()
    {
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