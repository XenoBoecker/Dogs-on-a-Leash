using System;
using System.Collections.Generic;
using UnityEngine;

public class LobbyDogSelector : MonoBehaviour
{
    LobbyData lobbyData;

    public string PlayerName;

    [SerializeField] int currentSelectedDogIndex;
    public int CurrentSelectedDogIndex => currentSelectedDogIndex;

    List<int> unlockedAccessories = new List<int>();
    int currentSelectedAccessorieIndex;
    public int CurrentSelectedAccessorieIndex => currentSelectedAccessorieIndex;

    bool isReadyToPlay;
    public bool IsReadyToPlay => isReadyToPlay;

    bool isSelectionConfirmed;
    public bool IsSelectionConfirmed => isSelectionConfirmed;

    public event Action OnDataChanged;

    private void Awake()
    {
        lobbyData = FindObjectOfType<LobbyData>();
    }

    private void Start()
    {
        unlockedAccessories = AchievementManager.Instance.GetUnlockedHatIndices();
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

    public void SelectNextAccessorie()
    {
        currentSelectedAccessorieIndex = GetNextUnlockedAccessorieIndex(currentSelectedAccessorieIndex);

        OnDataChanged?.Invoke();
    }

    public void SelectPreviousAccessorie()
    {
        currentSelectedAccessorieIndex = GetPreviousUnlockedAccessorieIndex(currentSelectedAccessorieIndex);

        OnDataChanged?.Invoke();
    }

    private int GetNextUnlockedAccessorieIndex(int currentIndex)
    {
        if(unlockedAccessories.Count == 0) return currentIndex;

        return unlockedAccessories[unlockedAccessories.IndexOf(currentIndex) + 1 >= unlockedAccessories.Count ? 0 : unlockedAccessories.IndexOf(currentIndex) + 1];
    }

    public int GetPreviousUnlockedAccessorieIndex(int currentIndex)
    {
        if (unlockedAccessories.Count == 0) return currentIndex;

        return unlockedAccessories[unlockedAccessories.IndexOf(currentIndex) - 1 < 0 ? unlockedAccessories.Count - 1 : unlockedAccessories.IndexOf(currentIndex) - 1];
    }

    public void SetSelectedAccessorieIndex(int i)
    {
        if (currentSelectedAccessorieIndex == i) return;

        currentSelectedAccessorieIndex = i;

        OnDataChanged?.Invoke();
    }

    internal DogData GetDogData()
    {
        if (lobbyData == null) lobbyData = FindObjectOfType<LobbyData>();
        // Debug.Log("GetDogData: available dogs: " + lobbyData.AvailableDogs.Length + " ; index: " + currentSelectedDogIndex);

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
