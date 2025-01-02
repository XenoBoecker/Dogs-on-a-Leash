using System;
using UnityEngine;
using UnityEngine.InputSystem;
// TODO: Assign Lobby player to LobbyDogSelector

public class LobbyPlayer : MonoBehaviour
{
    photonMenuLobby.LobbyManager lobbyManager;

    PlayerInput playerInput;

    string playerName;
    public string PlayerName => playerName;


    [SerializeField] DogData dogData;
    public DogData DogData => dogData;

    LobbyDogSelector lobbyDogSelector;

    bool isSelectionConfirmed;
    public bool IsSelectionConfirmed => isSelectionConfirmed;

    public event Action OnConfirmSelectionChanged;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        FindObjectOfType<LocalLobbyManager>()?.RegisterPlayer(this);

        lobbyManager = FindObjectOfType<photonMenuLobby.LobbyManager>();
        if (lobbyManager == null) Debug.LogError("No Lobby Manager found");
        lobbyManager.
            GetLocalClient()
            .AddLocalPlayer();
        //transform.SetParent(lobbyManager.playerItemParent);
    }

    private void OnEnable()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput)
        {
            Debug.Log("sub");
            playerInput.onActionTriggered += OnActionTriggered;
        }
    }
    private void OnDisable()
    {
        // Unsubscribe from the PlayerInput events
        var playerInput = GetComponent<PlayerInput>();
        if (playerInput)
        {
            playerInput.onActionTriggered -= OnActionTriggered;
        }
    }

    private void OnActionTriggered(InputAction.CallbackContext context)
    {
        Debug.Log("Action Triggered");

        // Handle the Move action
        if (context.action.name == "ChangeSelectedDog")
        {
            if(lobbyDogSelector == null)
            {
                Debug.Log("Not yet connected with selector for input");
                return;
            }
            if (lobbyDogSelector.IsSelectionConfirmed) return;

            if (context.phase == InputActionPhase.Started)
            {
                if (context.ReadValue<Vector2>().x < 0) lobbyDogSelector.SelectPreviousDog();
                else lobbyDogSelector.SelectNextDog();
            }
        }
        else if (context.phase == InputActionPhase.Performed) // Trigger toggle only on Performed
        {
            ToggleConfirmSelection();
        }
    }

    public void SetPlayerName(string name)
    {
        playerName = name;
    }

    public void SetLobbyDogSelector(LobbyDogSelector selector)
    {
        lobbyDogSelector = selector;
        lobbyDogSelector.PlayerName = playerName;
        lobbyDogSelector.SetIsPlayerControlled(true);

        lobbyDogSelector.OnDataChanged += UpdateDogData;

        lobbyDogSelector.DataChanged();
    }

    private void UpdateDogData()
    {
        dogData = lobbyDogSelector.GetDogData();
    }

    public void SelectNextDog()
    {
        lobbyDogSelector.SelectNextDog();
    }

    public void SelectPreviousDog()
    {
        lobbyDogSelector.SelectPreviousDog();
    }

    public void ToggleConfirmSelection()
    {
        isSelectionConfirmed = !isSelectionConfirmed;

        lobbyDogSelector?.SetConfirmSelection(isSelectionConfirmed);

        OnConfirmSelectionChanged?.Invoke();
    }
}