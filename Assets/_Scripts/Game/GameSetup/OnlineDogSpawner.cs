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

    public List<GameObject> dogModels = new List<GameObject>();

    internal void Setup()
    {
        LocalPlayer[] localPlayers = FindObjectsOfType<LocalPlayer>();

        for (int i = 0; i < localPlayers.Length; i++)
        {
            Dog dog;
            if (PhotonNetwork.IsConnected) dog = PhotonNetwork.Instantiate(dogPrefab.name, Vector3.zero, Quaternion.identity).GetComponent<Dog>();
            else dog = Instantiate(dogPrefab, spawnPoints[i].position, Quaternion.identity).GetComponent<Dog>();

            dog.SetDogData(localPlayers[i].DogData);

            dog.SetPlayerInput(localPlayers[i].GetComponent<PlayerInput>());

            OnDogSpawned?.Invoke(dog.transform);
        }

        
    }
}