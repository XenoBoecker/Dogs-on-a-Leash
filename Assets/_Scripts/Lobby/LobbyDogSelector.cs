using System;
using UnityEngine;

public class LobbyDogSelector : MonoBehaviour
{
    LobbyData lobbyData;

    public string PlayerName;
    int currentSelectedDogIndex;

    bool isPlayerControlled;
    public bool IsPlayerControlled => isPlayerControlled;
    bool isSelectionConfirmed;
    public bool IsSelectionConfirmed => isSelectionConfirmed;

    public event Action OnDataChanged;

    private void Awake()
    {
        lobbyData = FindObjectOfType<LobbyData>();
    }

    public void SelectNextDog()
    {
        currentSelectedDogIndex++;
        if (currentSelectedDogIndex >= lobbyData.AvailableDogs.Length) currentSelectedDogIndex = 0;

        DataChanged();
    }

    public void SelectPreviousDog()
    {
        currentSelectedDogIndex--;
        if (currentSelectedDogIndex < 0) currentSelectedDogIndex = lobbyData.AvailableDogs.Length - 1;

        DataChanged();
    }

    public void DataChanged()
    {
        OnDataChanged?.Invoke();
    }

    internal DogData GetDogData()
    {
        return lobbyData.AvailableDogs[currentSelectedDogIndex];
    }

    internal void SetIsPlayerControlled(bool isAIControlled)
    {
        this.isPlayerControlled = isAIControlled;
    }

    internal void SetConfirmSelection(bool isSelectionConfirmed)
    {
        this.isSelectionConfirmed = isSelectionConfirmed;

        DataChanged();
    }
}
