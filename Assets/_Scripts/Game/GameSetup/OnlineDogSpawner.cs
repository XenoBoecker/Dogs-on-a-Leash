using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OnlineDogSpawner : MonoBehaviour
{
    [SerializeField] Dog dogPrefab;

    [SerializeField] Transform[] spawnPoints;

    public event Action<Transform> OnDogSpawned;

    internal void Setup()
    {
        LocalPlayer[] localPlayers = FindObjectsOfType<LocalPlayer>();

        for (int i = 0; i < localPlayers.Length; i++)
        {
            Dog dog;
            if (PhotonNetwork.IsConnected) dog = PhotonNetwork.Instantiate(dogPrefab.name, Vector3.zero, Quaternion.identity).GetComponent<Dog>();
            else dog = Instantiate(dogPrefab, spawnPoints[i].position, Quaternion.identity).GetComponent<Dog>();

            Debug.Log("Local player " + i + ": DogDataIndex: "
                + localPlayers[i].
                DogData
                .id);

            dog.SetDogData(localPlayers[i].DogData);
            dog.SetColor(localPlayers[i].ColorIndex);
            dog.SetAccessorieIndex(localPlayers[i].AccessorieIndex);

            dog.SetPlayerInput(localPlayers[i].GetComponent<PlayerInput>());
            localPlayers[i].GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");

            OnDogSpawned?.Invoke(dog.transform);
        }
    }
}