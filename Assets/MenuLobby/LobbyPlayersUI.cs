using System.Collections.Generic;
using UnityEngine;

namespace photonMenuLobby
{
    public class LobbyPlayersUI : MonoBehaviour
    {
        LobbyManager lobbyManager;

        [SerializeField] List<LobbyShowDog> lobbyShowDogObjects;

        private void Awake()
        {
            lobbyManager = FindObjectOfType<LobbyManager>();
            lobbyManager.OnPlayerListChanged += UpdateUI;


            for (int i = 0; i < lobbyShowDogObjects.Count; i++)
            {
                lobbyShowDogObjects[i].gameObject.SetActive(false);
            }
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
                    lobbyShowDogObjects[index].SetPlayerData(lobbyManager.Clients[i].localPlayers[j]);
                    lobbyShowDogObjects[index].gameObject.SetActive(true);
                    index++;
                }
            }
        }
    }

}
