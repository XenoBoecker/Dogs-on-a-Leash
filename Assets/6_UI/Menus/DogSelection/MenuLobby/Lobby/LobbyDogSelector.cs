using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class LobbyDogSelector : MonoBehaviour
{
    [SerializeField] private HoldButton backButton;
    [SerializeField] private HoldButton leaveSceneBackButton;
    [SerializeField] private GameObject goToDogSelectionButton;
    LobbyData lobbyData;

    public string PlayerName;

    [SerializeField] int currentSelectedDogIndex;
    public int CurrentSelectedDogIndex => currentSelectedDogIndex;
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
        currentSelectedAccessorieIndex++;
        if (currentSelectedAccessorieIndex >= lobbyData.AvailableAccessoriesCount) currentSelectedAccessorieIndex = 0;

        OnDataChanged?.Invoke();
    }

    public void SelectPreviousAccessorie()
    {
        currentSelectedAccessorieIndex--;
        if (currentSelectedAccessorieIndex < 0) currentSelectedAccessorieIndex = lobbyData.AvailableAccessoriesCount - 1;

        OnDataChanged?.Invoke();
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

    internal void SetBackButtonSelected(bool isBackButtonSelected)
    {
        if (isBackButtonSelected)
        {
            EventSystem.current.SetSelectedGameObject(backButton.gameObject);
            backButton.FromScriptStartHold();
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(goToDogSelectionButton);
            backButton.FromScriptCancelHold();
        }
    }

    internal void SetLeaveSceneButtonSelected(bool isLeaveSceneButtonSelected)
    {
        if (isLeaveSceneButtonSelected)
        {
            EventSystem.current.SetSelectedGameObject(leaveSceneBackButton.gameObject);
            leaveSceneBackButton.FromScriptStartHold();
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(goToDogSelectionButton);
            leaveSceneBackButton.FromScriptCancelHold();
        }
    }
}
