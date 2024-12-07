using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{

    [SerializeField] string gameSceneName;

    List<LobbyPlayer> players = new List<LobbyPlayer>();
    [SerializeField] LobbyDogSelector[] lobbyDogSelectors;

    int currentPlayerCount;

    public event Action OnPlayerCountChanged;

    public void RegisterPlayer(LobbyPlayer player)
    {
        Debug.Log("Register player");

        players.Add(player);

        if (player.PlayerName == "") player.SetPlayerName("P" + (currentPlayerCount+1));
        
        player.SetLobbyDogSelector(lobbyDogSelectors[currentPlayerCount]);

        player.OnConfirmSelectionChanged += CheckAllPlayerConfirmed;

        currentPlayerCount++;

        OnPlayerCountChanged?.Invoke();
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
}
