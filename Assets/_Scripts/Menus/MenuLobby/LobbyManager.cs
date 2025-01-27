
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

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

        public bool IsInDogSelection;
        int readyToPlayDogCount;
        int seed;

        [SerializeField] List<LobbyDogSelector> chooseDogSelectors = new List<LobbyDogSelector>();
        public List<LobbyDogSelector> ChooseDogSelectors => chooseDogSelectors;


        [SerializeField] List<LobbyDogSelector> playerItemsList = new List<LobbyDogSelector>();
        [SerializeField] LobbyDogSelector playerItemPrefab;
        [SerializeField] public Transform playerItemParent;


        [SerializeField] GameObject playButton;

        [SerializeField] string gameSceneName = "Game";

        [SerializeField] bool testing;

        [SerializeField] int dogsNeededToStartGame = 4;
        int connectedDogCount;

        public event Action OnPlayerListChanged;
        public event Action OnBackToPlayerRegistration;

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

            seedInputField.onValueChanged.AddListener(OnSeedInputChanged);
        }

        private void Update()
        {
            if (PhotonNetwork.IsConnected)
            {
                if (PhotonNetwork.IsMasterClient)// && PhotonNetwork.CurrentRoom.PlayerCount >= 2)
                {
                    playButton.SetActive(true);
                }
                else
                {
                    playButton.SetActive(false);
                }

                if (Input.GetKeyDown(KeyCode.Return)) OnClickCreate();
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.L)) ActivatePanel(dogSelectionPanel);
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

        public void ReadyToPlayCountAdd(int i)
        {
            readyToPlayDogCount += i;

            if (readyToPlayDogCount == connectedDogCount) StartCoroutine(StartGameCountDown());
        }

        IEnumerator StartGameCountDown()
        {
            bool startGame = true;
            startGameButton.SetActive(true);

            for (int i = 0; i < countdownTime; i++)
            {
                countdownText.text = (countdownTime-i).ToString();

                yield return new WaitForSeconds(1);

                if(readyToPlayDogCount < connectedDogCount)
                {
                    startGame = false;
                    countdownText.text = "";
                    startGameButton.SetActive(false);
                }
            }

            if(startGame) FindObjectOfType<ChangeScenes>().LoadScene("Game_1");
        }

        void ActivatePanel(GameObject panel)
        {
            if (panel == lobbyPanel) lobbyPanel.SetActive(true);
            else lobbyPanel.SetActive(false);

            if (panel == roomPanel) roomPanel.SetActive(true);
            else roomPanel.SetActive(false);

            if (panel == dogSelectionPanel)
            {
                connectedDogCount = GetCurrentPlayerCount();
                IsInDogSelection = true;

                dogSelectionPanel.SetActive(true);
                dogModelParent.SetActive(true);
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
                if (FindObjectsOfType<LocalPlayer>().Length == dogsNeededToStartGame) ActivatePanel(dogSelectionPanel);
            }

            OnPlayerListChanged?.Invoke();
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
    }

}
