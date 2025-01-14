using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OnlineDogSpawner : MonoBehaviour
{
    [SerializeField] Dog dogPrefab;

    public event Action<Transform> OnDogSpawned;

    public List<GameObject> dogModels = new List<GameObject>();

    internal void Setup()
    {
        LocalPlayer[] localPlayers = FindObjectsOfType<LocalPlayer>();

        for (int i = 0; i < localPlayers.Length; i++)
        {
            Dog dog;
            if (PhotonNetwork.IsConnected) dog = PhotonNetwork.Instantiate(dogPrefab.name, Vector3.zero, Quaternion.identity).GetComponent<Dog>();
            else dog = Instantiate(dogPrefab, new Vector3(i, 0, 0), Quaternion.identity).GetComponent<Dog>();

            dog.SetDogData(localPlayers[i].DogData);

            dog.GetComponent<MeshFilter>().mesh = dogModels[i].GetComponent<MeshFilter>().sharedMesh;

            dog.SetPlayerInput(localPlayers[i].GetComponent<PlayerInput>());

            OnDogSpawned?.Invoke(dog.transform);
        }

        
    }
}