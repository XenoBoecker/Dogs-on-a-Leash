
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;

namespace photonMenuLobby
{

    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] TMP_InputField roomInputField;

        [SerializeField] GameObject lobbyPanel;
        [SerializeField] GameObject roomPanel;
        [SerializeField] TMP_Text roomNameText;

        [SerializeField] RoomItem roomItemPrefab;

        List<RoomItem> roomItemsList = new List<RoomItem>();
        [SerializeField] Transform contentObject;

        [SerializeField] float timeBetweenUpdates = 1.5f;
        float nextUpdateTime;




        List<Client> clients = new List<Client>();
        public List<Client> Clients => clients;

        [SerializeField] Client clientPrefab;

        [SerializeField] Transform clientParent;





        [SerializeField] List<LobbyDogSelector> playerItemsList = new List<LobbyDogSelector>();
        [SerializeField] LobbyDogSelector playerItemPrefab;
        [SerializeField] public Transform playerItemParent;

        [SerializeField] GameObject playButton;

        [SerializeField] string gameSceneName = "Game";

        [SerializeField] bool testing;

        public event Action OnPlayerListChanged;

        private void Start()
        {
            roomPanel.SetActive(false);

            PhotonNetwork.JoinLobby();
        }

        private void Update()
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
            lobbyPanel.SetActive(false);
            roomPanel.SetActive(true);

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
            lobbyPanel.SetActive(true);
            roomPanel.SetActive(false);
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

        void UpdatePlayerList()
        {
            Debug.Log("UpdatePlayerList");

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
                }
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

        public void OnClickPlayButton()
        {
            PhotonNetwork.LoadLevel(gameSceneName);
        }

        public Client GetLocalClient()
        {
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].IsLocal) return clients[i];
            }
            return null;
        }
    }

    public class LobbyPlayersUI : MonoBehaviour
    {
        LobbyManager lobbyManager;

        [SerializeField] List<LobbyShowDog> lobbyShowDogObjects;

        private void Awake()
        {
            lobbyManager = FindObjectOfType<LobbyManager>();
            lobbyManager.OnPlayerListChanged += UpdateUI;
        }

        private void UpdateUI()
        {
            for (int i = 0; i < lobbyShowDogObjects.Count; i++)
            {
                lobbyShowDogObjects[i].gameObject.SetActive(false);
            }

            int index = 0;

            for (int i = 0; i < lobbyManager.Clients.Count; i++)
            {
                for (int j = 0; j < lobbyManager.Clients[i].localPlayers.Count; j++)
                {
                    lobbyShowDogObjects[index++].SetPlayerData(lobbyManager.Clients[i].localPlayers[j]);
                    lobbyShowDogObjects[index++].gameObject.SetActive(true);
                }
            }
        }
    }

}
