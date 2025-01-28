using photonMenuLobby;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogModelPositions : MonoBehaviour
{
    LobbyManager lobbyManager;

    [SerializeField] List<GameObject> dogParents;

    [SerializeField] Transform oneDog;
    [SerializeField] Transform[] twoDogs, threeDogs, fourDogs;

    // Start is called before the first frame update
    void Start()
    {
        lobbyManager = FindObjectOfType<LobbyManager>();
        lobbyManager.OnPlayerListChanged += UpdateDogPositions;

    }

    private void UpdateDogPositions()
    {
        int currentPlayerCount = FindObjectsOfType<LocalPlayer>().Length;

        for (int i = 0; i < dogParents.Count; i++)
        {
            if (i < currentPlayerCount) dogParents[i].SetActive(true);
            else dogParents[i].SetActive(false);
        }

        if (currentPlayerCount == 1) dogParents[0].transform.position = oneDog.position;
        else if (currentPlayerCount == 2) SetPositions(currentPlayerCount, twoDogs);
        else if (currentPlayerCount == 3) SetPositions(currentPlayerCount, threeDogs);
        else if (currentPlayerCount == 4) SetPositions(currentPlayerCount, fourDogs);
    }

    private void SetPositions(int currentPlayerCount, Transform[] transformList)
    {
        for (int i = 0; i < currentPlayerCount; i++)
        {
            dogParents[i].transform.position = transformList[i].position;
        }
    }
}
