
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace photonMenuLobby
{

    public class LobbyManager : MonoBehaviourPunCallbacks
    {

        [SerializeField] PlayerInputManager pim;

        [SerializeField] TMP_InputField roomInputField;

        [SerializeField] TMP_InputField seedInputField;

        [SerializeField] GameObject lobbyPanel;
        [SerializeField] GameObject roomPanel;
        [SerializeField] GameObject dogSelectionPanel;

        [SerializeField] GameObject dogModelParent;
        [SerializeField] TMP_Text roomNameText;


        [SerializeField] GameObject startGameButton;
        [SerializeField] TMP_Text countdownText;

        [SerializeField] int countdownTime = 5;

        [SerializeField] RoomItem roomItemPrefab;

        List<RoomItem> roomItemsList = new List<RoomItem>();
        [SerializeField] Transform contentObject;

        [SerializeField] float timeBetweenUpdates = 1.5f;
        float nextUpdateTime;




        List<Client> clients = new List<Client>();
        public List<Client> Clients => clients;

        [SerializeField] Client clientPrefab;

        [SerializeField] Transform clientParent;

        bool startGame;
        public bool IsInDogSelection;
        int readyToPlayDogCount;
        int seed;

        [SerializeField] List<LobbyDogSelector> chooseDogSelectors = new List<LobbyDogSelector>();
        public List<LobbyDogSelector> ChooseDogSelectors => chooseDogSelectors;


        [SerializeField] List<LobbyDogSelector> playerItemsList = new List<LobbyDogSelector>();
        [SerializeField] LobbyDogSelector playerItemPrefab;
        [SerializeField] public Transform playerItemParent;


        [SerializeField] GameObject playButton;

        [SerializeField] GameObject hiddenButton;

        [SerializeField] string gameSceneName = "Game";

        [SerializeField] bool testing;

        [SerializeField] int dogsNeededToStartGame = 4;

        [SerializeField] float switchPanelDelayAfterAllPlayersRegistered = 0.5f;
        int connectedDogCount;

        // UI navigation control variables
        private Button playButtonComponent;
        private bool originalPlayButtonInteractable;
        private Navigation originalPlayButtonNavigation;
        
        // Delay to prevent accidental activation right after player joins
        private float playButtonActivationDelay = 1.0f;
        private float lastPlayerJoinTime;

        public event Action OnPlayerListChanged;
        public event Action OnBackToPlayerRegistration;

        private void Awake()
        {
            seedInputField.onValueChanged.AddListener(OnSeedInputChanged);
            
            // Cache the play button component and its original settings
            if (playButton != null)
            {
                playButtonComponent = playButton.GetComponent<Button>();
                if (playButtonComponent != null)
                {
                    originalPlayButtonInteractable = playButtonComponent.interactable;
                    originalPlayButtonNavigation = playButtonComponent.navigation;
                }
            }
        }

        private void Start()
        {
            DeletePreviousLocalPlayers();

            if (PhotonNetwork.IsConnected)
            {
                ActivatePanel(lobbyPanel);

                PhotonNetwork.JoinLobby();
            }
            else
            {
                ActivatePanel(roomPanel);
                
                playButton.SetActive(true);

                pim.enabled = true;
                startGameButton.SetActive(false);
                roomNameText.text = "Room Name: " + "TestName";
                countdownText.text = "";
                UpdateClientList();
                UpdatePlayerList();
            }

            seedInputField.text = UnityEngine.Random.Range(100000000, 999999999).ToString();
        }

        private void Update()
        {
            if (PhotonNetwork.IsConnected)
            {
                if (PhotonNetwork.IsMasterClient)// && PhotonNetwork.CurrentRoom.PlayerCount >= 2)
                {
                    playButton.SetActive(true);
                    UpdatePlayButtonVisualState();
                }
                else
                {
                    playButton.SetActive(false);
                }

                if (Input.GetKeyDown(KeyCode.Return)) OnClickCreate();
            }
            else
            {
                // In offline mode, also apply the delay
                UpdatePlayButtonVisualState();
            }

            // if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.L)) ActivatePanel(dogSelectionPanel); // Hacks

            // Only set the hidden button as selected if we're in dog selection phase
            // This prevents accidental navigation to UI buttons while players are joining in the room panel
            if (EventSystem.current.currentSelectedGameObject == null && IsInDogSelection) 
            {
                EventSystem.current.SetSelectedGameObject(hiddenButton);
            }
        }

        void SetSeed(int v)
        {
            // Debug.Log("Set Seed: " + v);

            seed = v;

            seedInputField.text = seed.ToString();

            PlayerPrefs.SetInt("Seed", seed);
        }

        void OnSeedInputChanged(string input)
        {
            Debug.Log("Set Seed to " + input);

            // Attempt to parse the string input to an integer
            if (int.TryParse(input, out int parsedSeed))
            {
                SetSeed(parsedSeed);
            }
            else
            {
                Debug.LogWarning("Invalid seed input. Please enter a valid integer.");
            }
        }

        public void DeletePreviousLocalPlayers()
        {
            LocalPlayer[] localPlayers = FindObjectsOfType<LocalPlayer>();

            for (int i = 0; i < localPlayers.Length; i++)
            {
                Destroy(localPlayers[i].gameObject);
            }
        }

        public void CheckReadyToPlay()
        {
            LocalPlayer[] localPlayers = FindObjectsOfType<LocalPlayer>();

            connectedDogCount = localPlayers.Length;

            readyToPlayDogCount = 0;
            for (int i = 0; i < connectedDogCount; i++)
            {

                if (localPlayers[i].IsReadyToPlay)
                {
                    readyToPlayDogCount++;
                }
            }

            if (readyToPlayDogCount < connectedDogCount) return;
            
            StartCoroutine(StartGameCountDown());
        }

        IEnumerator StartGameCountDown()
        {
            if (startGame) yield break;

            startGame = true;
            startGameButton.SetActive(true);

            for (float i = 0; i < countdownTime; i+=Time.unscaledDeltaTime)
            {
                int nbr = (countdownTime - (int)i);

                if (countdownText.text != nbr.ToString()) SoundManager.Instance.PlaySound(SoundManager.Instance.uiSFX.dogSelectionCountdownTick);

                countdownText.text = nbr.ToString();

                if(readyToPlayDogCount < connectedDogCount)
                {
                    startGame = false;
                    countdownText.text = "";
                    startGameButton.SetActive(false);
                    yield break;
                }
                yield return null;
            }
            countdownText.text = "0";

            if (startGame)
            {
                SoundManager.Instance.PlaySound(SoundManager.Instance.uiSFX.dogSelectionCountdownFinish);

                Debug.Log("Start game");
                OnSeedInputChanged(seedInputField.text);
                FindObjectOfType<ChangeScenes>().LoadScene("Game_1");
            }
        }

        void ActivatePanel(GameObject panel)
        {
            if (panel == lobbyPanel) lobbyPanel.SetActive(true);
            else lobbyPanel.SetActive(false);

            if (panel == roomPanel) 
            {
                roomPanel.SetActive(true);
                // Protect UI from accidental input when players are joining
                ProtectUIFromAccidentalInput();
            }
            else roomPanel.SetActive(false);

            if (panel == dogSelectionPanel)
            {
                connectedDogCount = GetCurrentPlayerCount();
                IsInDogSelection = true;

                dogSelectionPanel.SetActive(true);
                dogModelParent.SetActive(true);
                
                // Restore normal UI behavior in dog selection
                RestoreNormalUIBehavior();
            }
            else
            {
                IsInDogSelection = false;
                dogSelectionPanel.SetActive(false);
                dogModelParent.SetActive(false);
            }
        }

        public void BackToPlayerRegistration()
        {
            ActivatePanel(roomPanel);

            OnBackToPlayerRegistration?.Invoke();
        }

        public void GoToDogSelectionPanel()
        {
            // Prevent accidental activation if called too soon after player joins
            if (!ShouldPlayButtonBeInteractable())
            {
                Debug.Log("GoToDogSelectionPanel blocked - too soon after player joined");
                return;
            }

            ActivatePanel(dogSelectionPanel);

            SetSeed(UnityEngine.Random.Range(100000000, 999999999));
        }

        public void OnClickCreate()
        {
            if (roomInputField.text.Length >= 1)
            {
                PhotonNetwork.CreateRoom(roomInputField.text, new RoomOptions() { MaxPlayers = 8, BroadcastPropsChangeToAll = true });
            }
        }

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();


            if (testing)
            {
                PhotonNetwork.CreateRoom("test Room", new RoomOptions() { MaxPlayers = 8, BroadcastPropsChangeToAll = true });
            }
        }

        public override void OnJoinedRoom()
        {
            ActivatePanel(roomPanel);

            pim.enabled = true;

            roomNameText.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;
            UpdateClientList();
            UpdatePlayerList();

            if (testing)
            {
                PhotonNetwork.LoadLevel(gameSceneName);
            }
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            if (Time.time >= nextUpdateTime)
            {
                UpdateRoomList(roomList);
                nextUpdateTime = Time.time + timeBetweenUpdates;
            }
        }

        private void UpdateRoomList(List<RoomInfo> roomList)
        {
            foreach (RoomItem item in roomItemsList)
            {
                Destroy(item.gameObject);
            }
            roomItemsList.Clear();

            foreach (RoomInfo room in roomList)
            {
                RoomItem newRoom = Instantiate(roomItemPrefab, contentObject);
                newRoom.SetRoomName(room.Name);
                roomItemsList.Add(newRoom);
            }

        }

        internal void JoinRoom(string roomName)
        {
            PhotonNetwork.JoinRoom(roomName);
        }

        public void OnClickLeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public override void OnLeftRoom()
        {
            ActivatePanel(lobbyPanel);
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
        }

        void UpdateClientList()
        {
            foreach (Client client in clients)
            {
                client.OnPlayerDataChanged -= UpdatePlayerList;
                Destroy(client.gameObject);
            }
            clients.Clear();

            // Track when players are joining to add delay
            lastPlayerJoinTime = Time.time;

            if (PhotonNetwork.IsConnected)
            {
                if (PhotonNetwork.CurrentRoom == null) return;

                foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
                {
                    Client newClient = Instantiate(clientPrefab, clientParent);
                    newClient.name = player.Value.NickName;
                    newClient.SetPlayerInfo(player.Value);

                    if (player.Value == PhotonNetwork.LocalPlayer)
                    {
                        newClient.SetIsLocalPlayer();
                    }

                    newClient.OnPlayerDataChanged += UpdatePlayerList;

                    clients.Add(newClient);
                }
            }
            else
            {
                Client newClient = Instantiate(clientPrefab, clientParent);
                newClient.SetIsLocalPlayer();
                newClient.OnPlayerDataChanged += UpdatePlayerList;
                clients.Add(newClient);
            }
        }

        void UpdatePlayerList()
        {
            // Debug.Log("UpdatePlayerList");
            
            // Track when player list updates (which happens when local players join)
            lastPlayerJoinTime = Time.time;

            int currentPlayerCount = 0;

            if (PhotonNetwork.IsConnected)
            {
                foreach (LobbyDogSelector item in playerItemsList)
                {
                    Destroy(item.gameObject);
                }
                playerItemsList.Clear();

                if (PhotonNetwork.CurrentRoom == null) return;

                foreach (Client client in clients)
                {
                    if (client.IsLocal) continue;

                    Debug.Log("Spawn Clients local players: " + client.localPlayers.Count);

                    for (int i = 0; i < client.localPlayers.Count; i++)
                    {
                        LobbyDogSelector newPlayerItem = Instantiate(playerItemPrefab, playerItemParent);

                        newPlayerItem.SetSelectedDogIndex(client.localPlayers[i].playerAvatar);

                        playerItemsList.Add(newPlayerItem);

                        currentPlayerCount++;
                    }
                }


                if (currentPlayerCount == dogsNeededToStartGame)
                {
                    ActivatePanel(dogSelectionPanel);
                }
            }
            else
            {
                currentPlayerCount = FindObjectsOfType<LocalPlayer>().Length;

                for (int i = 0; i < chooseDogSelectors.Count; i++)
                {
                    if (i < currentPlayerCount) chooseDogSelectors[i].gameObject.SetActive(true);
                    else chooseDogSelectors[i].gameObject.SetActive(false);
                }

                if (currentPlayerCount == dogsNeededToStartGame)
                {
                    StartCoroutine(DelayedDogSelectionPanelActivation());
                }
            }
            OnPlayerListChanged?.Invoke();
        }

        private IEnumerator DelayedDogSelectionPanelActivation()
        {
            yield return new WaitForSeconds(switchPanelDelayAfterAllPlayersRegistered);

            ActivatePanel(dogSelectionPanel);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            UpdateClientList();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            UpdateClientList();
        }
        void OnDestroy()
        {
            // Unsubscribe to avoid potential memory leaks
            seedInputField.onValueChanged.RemoveListener(OnSeedInputChanged);
        }

        public Client GetLocalClient()
        {
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].IsLocal) return clients[i];
            }
            return null;
        }

        internal int GetCurrentPlayerCount()
        {
            int count = 0;

            for (int i = 0; i < clients.Count; i++)
            {
                count += clients[i].localPlayers.Count;
            }
            return count;
        }

        // Methods to prevent accidental UI navigation during player joining
        private void DisablePlayButtonNavigation()
        {
            if (playButtonComponent != null)
            {
                Navigation nav = playButtonComponent.navigation;
                nav.mode = Navigation.Mode.None;
                playButtonComponent.navigation = nav;
            }
        }

        private void EnablePlayButtonNavigation()
        {
            if (playButtonComponent != null)
            {
                playButtonComponent.navigation = originalPlayButtonNavigation;
            }
        }

        private bool ShouldPlayButtonBeInteractable()
        {
            // Add delay after player joins to prevent accidental activation
            bool timeDelayPassed = Time.time - lastPlayerJoinTime > playButtonActivationDelay;
            
            // Also check if we have enough players (optional additional safety)
            int currentPlayers = GetCurrentPlayerCount();
            bool hasMinimumPlayers = currentPlayers >= 1; // At least one player needed
            
            return timeDelayPassed && hasMinimumPlayers;
        }

        private void UpdatePlayButtonVisualState()
        {
            if (playButtonComponent != null)
            {
                bool shouldBeInteractable = ShouldPlayButtonBeInteractable();
                playButtonComponent.interactable = shouldBeInteractable;
                
                // Optional: You could add visual feedback here, like changing alpha or color
                // to indicate when the button will become available
            }
        }

        // Call this when switching to room panel to protect against accidental input
        private void ProtectUIFromAccidentalInput()
        {
            DisablePlayButtonNavigation();
            
            // Clear any current UI selection to prevent accidental activation
            if (EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        // Call this when entering dog selection to restore normal UI behavior
        private void RestoreNormalUIBehavior()
        {
            EnablePlayButtonNavigation();
        }
    }

}
