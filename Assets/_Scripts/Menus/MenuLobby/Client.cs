
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace photonMenuLobby
{
    public class Client : MonoBehaviourPunCallbacks
    {
        bool isLocal;
        public bool IsLocal => isLocal;
        Player player;

        public List<PlayerData> localPlayers = new List<PlayerData>();

        string localPlayerCountString = "localPlayers.Count";
        string avatarString = "avatar";

        Hashtable playerProperties = new Hashtable();

        public event Action OnPlayerDataChanged;

        public void AddLocalPlayer()
        {
            // Debug.Log("Add local Player to player " + PhotonNetwork.LocalPlayer.ActorNumber);

            localPlayers.Add(new PlayerData(0));

            if (PhotonNetwork.IsConnected)
            {
                for (int i = 0; i < localPlayers.Count; i++)
                {
                    string s = localPlayerCountString + avatarString + i;

                    playerProperties[s] = localPlayers[i].playerAvatar;
                }

                playerProperties[localPlayerCountString] = localPlayers.Count;

                PhotonNetwork.SetPlayerCustomProperties(playerProperties);
            }
            else
            {
                OnPlayerDataChanged?.Invoke();
            }
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            Debug.Log("Player " + targetPlayer.ActorNumber + " properties updated");

            if (player == targetPlayer)
            {
                Debug.Log("my player got updated");

                UpdateClientInfo(targetPlayer);
            }

            OnPlayerDataChanged?.Invoke();
        }

        internal void SetIsLocalPlayer()
        {
            isLocal = true;
        }

        internal void SetPlayerInfo(Player value)
        {
            player = value;
            name = "Client " + player.ActorNumber;
        }

        private void UpdateClientInfo(Player targetPlayer)
        {
            if (targetPlayer.CustomProperties.ContainsKey(localPlayerCountString))
            {
                localPlayers.Clear();

                int localPlayerCount = (int)targetPlayer.CustomProperties[localPlayerCountString];
                for (int i = 0; i < localPlayerCount; i++)
                {
                    string s = localPlayerCountString + avatarString + i;
                    int playerAvatar = (int)targetPlayer.CustomProperties[s];

                    localPlayers.Add(new PlayerData(playerAvatar));
                }

                Debug.Log("Local player (" + player.ActorNumber + ") Count: " + localPlayers.Count);
            }
        }
    }

}
