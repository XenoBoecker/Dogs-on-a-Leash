using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace photonMenuLobby
{
    public class LobbyPlayersUI : MonoBehaviour
    {
        LobbyManager lobbyManager;

        [SerializeField] List<GameObject> pawShowPlayerConnected;


        [SerializeField] AnimationCurve popCurve;

        [SerializeField] float popDuration = 0.3f;

        List<Transform> hasPoppedAlready = new List<Transform>();

        private void Awake()
        {
            lobbyManager = FindObjectOfType<LobbyManager>();
            lobbyManager.OnPlayerListChanged += UpdateUI;


            for (int i = 0; i < pawShowPlayerConnected.Count; i++)
            {
                pawShowPlayerConnected[i].gameObject.SetActive(false);
            }
        }

        private void UpdateUI()
        {
            for (int i = 0; i < pawShowPlayerConnected.Count; i++)
            {
                pawShowPlayerConnected[i].gameObject.SetActive(false);
            }

            int index = 0;

            for (int i = 0; i < lobbyManager.Clients.Count; i++)
            {
                for (int j = 0; j < lobbyManager.Clients[i].localPlayers.Count; j++)
                {
                    //lobbyShowDogObjects[index].SetPlayerData(lobbyManager.Clients[i].localPlayers[j]);
                    pawShowPlayerConnected[index].gameObject.SetActive(true);
                    StartCoroutine(Pop(pawShowPlayerConnected[index].transform));
                    index++;
                }
            }
        }
        IEnumerator Pop(Transform t)
        {
            if (hasPoppedAlready.Contains(t)) yield break;

            // Debug.Log("Start Pop: 2)" + t.name + "; duration: " + popDuration);

            hasPoppedAlready.Add(t);

            Vector3 startScale = t.localScale;
            for (float i = 0; i < popDuration; i+= Time.deltaTime)
            {
                t.localScale = startScale * popCurve.Evaluate(i / popDuration);

                // Debug.Log("Scale: " +t.localScale);
                yield return null;
            }
        }
    }

}