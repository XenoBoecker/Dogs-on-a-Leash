using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class OnlineDogSpawner : MonoBehaviour
{
    [SerializeField] Dog dogPrefab;

    public event Action<Transform> OnDogSpawned;

    internal void Setup()
    {
        LocalPlayer[] localPlayers = FindObjectsOfType<LocalPlayer>();

        for (int i = 0; i < localPlayers.Length; i++)
        {
            Dog dog;
            if (PhotonNetwork.IsConnected) dog = PhotonNetwork.Instantiate(dogPrefab.name, Vector3.zero, Quaternion.identity).GetComponent<Dog>();
            else dog = Instantiate(dogPrefab, Vector3.zero, Quaternion.identity).GetComponent<Dog>();

            dog.SetDogData(localPlayers[i].DogData);

            dog.SetPlayerInput(localPlayers[i].GetComponent<PlayerInput>());

            OnDogSpawned?.Invoke(dog.transform);
        }
    }
}