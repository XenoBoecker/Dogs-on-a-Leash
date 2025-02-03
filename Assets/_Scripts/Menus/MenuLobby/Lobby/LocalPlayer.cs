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
    public int AccessorieIndex;


    [SerializeField] float controllerInputDeadZone = 0.3f;

    [SerializeField] float selectNextDogDelay = 0.2f;
    float nextDogWaitTimer;

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

        lobbyManager.OnBackToPlayerRegistration += ResetDogSelection;

        SetLobbyDogSelector(lobbyManager.ChooseDogSelectors[lobbyManager.GetCurrentPlayerCount() - 1]);

        ColorIndex = lobbyManager.GetCurrentPlayerCount() - 1;
    }

    private void Update()
    {
        if (lobbyManager.IsInDogSelection) waitTimeBeforeCanConfirmSelection -= Time.deltaTime;
        else waitTimeBeforeCanConfirmSelection = 0.1f;

        nextDogWaitTimer -= Time.deltaTime;
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
            if (lobbyDogSelector.IsReadyToPlay) return;
            else
            {
                if (context.phase == InputActionPhase.Performed)
                {
                    if (nextDogWaitTimer > 0) return;

                    float x = context.ReadValue<Vector2>().x;
                    float y = context.ReadValue<Vector2>().y;

                    if (Mathf.Abs(x) < controllerInputDeadZone && Mathf.Abs(y) < controllerInputDeadZone) return;

                    nextDogWaitTimer = selectNextDogDelay;


                    if (lobbyDogSelector.IsSelectionConfirmed)
                    {
                        if (Mathf.Abs(x) > Mathf.Abs(y))
                        {
                            if (x < -controllerInputDeadZone) lobbyDogSelector.SelectPreviousAccessorie();
                            else if (x > controllerInputDeadZone) lobbyDogSelector.SelectNextAccessorie();
                        }
                        else
                        {
                            if (y < -controllerInputDeadZone) lobbyDogSelector.SelectPreviousAccessorie();
                            else if (y > controllerInputDeadZone) lobbyDogSelector.SelectNextAccessorie();
                        }
                    }
                    else
                    {
                        if (Mathf.Abs(x) > Mathf.Abs(y))
                        {
                            if (x < -controllerInputDeadZone)
                            {
                                lobbyDogSelector.SelectPreviousDog();
                            }
                            else if (x > controllerInputDeadZone)
                            {
                                lobbyDogSelector.SelectNextDog();
                            }
                        }
                        else
                        {
                            if (y < -controllerInputDeadZone) lobbyDogSelector.SelectPreviousDog();
                            else if (y > controllerInputDeadZone) lobbyDogSelector.SelectNextDog();
                        }
                    }

                }
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
        else if (context.action.name == "ExitSelection")
        {
            if (context.phase == InputActionPhase.Performed) // Trigger toggle only on Performed
            {
                ExitDogSelection();
            }
        }
    }

    private void ResetDogSelection()
    {
        UnConfirmSelection();
        UnConfirmSelection();

        lobbyDogSelector.SetSelectedDogIndex(0);
        lobbyDogSelector.SetSelectedAccessorieIndex(0);
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

        AccessorieIndex = lobbyDogSelector.CurrentSelectedAccessorieIndex;

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


        if (!isSelectionConfirmed)
        {
            isSelectionConfirmed = true;
            SoundManager.Instance.PlaySound(SoundManager.Instance.uiSFX.confirmDogSelection);
        }
        else
        {
            if(!isReadyToPlay) SoundManager.Instance.PlaySound(SoundManager.Instance.uiSFX.confirmAccessorySelection);
            Debug.Log("Ready to play " + name);
            isReadyToPlay = true;
            lobbyManager.CheckReadyToPlay();
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
            lobbyManager.CheckReadyToPlay();
        }
        else isSelectionConfirmed = false;

        lobbyDogSelector?.SetConfirmSelection(isSelectionConfirmed);
        lobbyDogSelector?.SetReadyToPlay(isReadyToPlay);

        OnConfirmSelectionChanged?.Invoke();
    }

    private void ExitDogSelection()
    {
        lobbyManager.BackToPlayerRegistration();
    }
}