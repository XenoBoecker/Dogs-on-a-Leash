using System;
using UnityEngine;

public class LobbyDogSelector : MonoBehaviour
{
    LobbyData lobbyData;

    public string PlayerName;
    int currentSelectedDogIndex;
    public int CurrentSelectedDogIndex => currentSelectedDogIndex;

    bool isReadyToPlay;
    public bool IsReadyToPlay => isReadyToPlay;

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

        OnDataChanged?.Invoke();
    }

    public void SelectPreviousDog()
    {
        currentSelectedDogIndex--;
        if (currentSelectedDogIndex < 0) currentSelectedDogIndex = lobbyData.AvailableDogs.Length - 1;

        OnDataChanged?.Invoke();
    }

    public void SetSelectedDogIndex(int i)
    {
        if (currentSelectedDogIndex == i) return;

        currentSelectedDogIndex = i;

        OnDataChanged?.Invoke();
    }

    internal DogData GetDogData()
    {
        return lobbyData.AvailableDogs[currentSelectedDogIndex];
    }

    internal void SetConfirmSelection(bool isSelectionConfirmed)
    {
        if (this.isSelectionConfirmed == isSelectionConfirmed) return;

        this.isSelectionConfirmed = isSelectionConfirmed;

        OnDataChanged?.Invoke();
    }

    internal void SetReadyToPlay(bool isReadyToPlay)
    {
        if (this.isReadyToPlay == isReadyToPlay) return;

        this.isReadyToPlay = isReadyToPlay;

        OnDataChanged?.Invoke();
    }
}
