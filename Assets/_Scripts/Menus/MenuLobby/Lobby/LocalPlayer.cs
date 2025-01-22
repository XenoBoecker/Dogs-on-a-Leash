using System;
using UnityEngine;
using UnityEngine.InputSystem;
// TODO: Assign Lobby player to LobbyDogSelector

public class LocalPlayer : MonoBehaviour
{
    photonMenuLobby.LobbyManager lobbyManager;

    PlayerInput playerInput;

    string playerName;
    public string PlayerName => playerName;

    [SerializeField] DogData dogData;
    public DogData DogData => dogData;

    public int ColorIndex;

    LobbyDogSelector lobbyDogSelector;

    bool isReadyToPlay;
    public bool IsReadyToPlay => isReadyToPlay;

    bool isSelectionConfirmed;
    public bool IsSelectionConfirmed => isSelectionConfirmed;

    float waitTimeBeforeCanConfirmSelection = 0.1f;

    public event Action OnConfirmSelectionChanged;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // FindObjectOfType<LocalLobbyManager>()?.RegisterPlayer(this);

        lobbyManager = FindObjectOfType<photonMenuLobby.LobbyManager>();
        if (lobbyManager == null) Debug.LogError("No Lobby Manager found");
        lobbyManager.
            GetLocalClient()
            .AddLocalPlayer();
        //transform.SetParent(lobbyManager.playerItemParent);

        SetLobbyDogSelector(lobbyManager.ChooseDogSelectors[lobbyManager.GetCurrentPlayerCount() - 1]);

        ColorIndex = lobbyManager.GetCurrentPlayerCount() - 1;
    }
    private void Update()
    {
        if (lobbyManager.IsInDogSelection) waitTimeBeforeCanConfirmSelection -= Time.deltaTime;
        else waitTimeBeforeCanConfirmSelection = 0.1f;
    }

    private void OnEnable()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput)
        {
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
        // Debug.Log("Action Triggered: " + context.action.name);

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
        else if (context.action.name == "ConfirmSelection")
        {
            if (context.phase == InputActionPhase.Performed) // Trigger toggle only on Performed
            {
                ConfirmSelection();
            }
        }else if(context.action.name == "Back")
        {
            if (context.phase == InputActionPhase.Performed) // Trigger toggle only on Performed
            {
                UnConfirmSelection();
            }
        }
    }

    public void SetLobbyDogSelector(LobbyDogSelector selector)
    {
        // Debug.Log("LocalPlayer Set Lobby Dog Selector");

        lobbyDogSelector = selector;

        lobbyDogSelector.OnDataChanged += UpdateDogData;

        // if (selector == null) Debug.Log("no selector set");
        // else Debug.Log("dogdata: " + lobbyDogSelector.GetDogData().name);
        
        UpdateDogData();
    }

    private void UpdateDogData()
    {
        dogData = 
            lobbyDogSelector
            .GetDogData();

        // Debug.Log("Local player Dog data changed: " + dogData.id);
    }

    public void SelectNextDog()
    {
        // Debug.Log("Select next");
        lobbyDogSelector.SelectNextDog();
    }

    public void SelectPreviousDog()
    {
        // Debug.Log("select previous");
        lobbyDogSelector.SelectPreviousDog();
    }

    public void ConfirmSelection()
    {
        if (waitTimeBeforeCanConfirmSelection > 0) return;

        //  Debug.Log("Confirm Selection");
        if (!isSelectionConfirmed) isSelectionConfirmed = true;
        else
        {
            isReadyToPlay = true;
            lobbyManager.ReadyToPlayCountAdd(1);
        }

        lobbyDogSelector?.SetConfirmSelection(isSelectionConfirmed);
        lobbyDogSelector?.SetReadyToPlay(isReadyToPlay);


        OnConfirmSelectionChanged?.Invoke();
    }

    private void UnConfirmSelection()
    {
        // Debug.Log("Unconfirm Selection");
        if (isReadyToPlay)
        {
            isReadyToPlay = false;
            lobbyManager.ReadyToPlayCountAdd(-1);
        }
        else isSelectionConfirmed = false;

        lobbyDogSelector?.SetConfirmSelection(isSelectionConfirmed);
        lobbyDogSelector?.SetReadyToPlay(isReadyToPlay);

        OnConfirmSelectionChanged?.Invoke();
    }
}