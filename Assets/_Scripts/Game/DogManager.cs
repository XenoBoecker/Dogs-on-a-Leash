using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DogManager : MonoBehaviour
{
    public event Action<Transform> OnDogSpawned;
    public event Action<Transform> OnDogDespawned;

    private void Start()
    {
        DogController[] dogControllers = FindObjectsOfType<DogController>(true);
        
        foreach (DogController dogController in dogControllers)
        {
            dogController.GetComponent<PlayerInput>().currentActionMap = dogController.GetComponent<PlayerInput>().actions.FindActionMap("Player");
            dogController.enabled = true;
            dogController.GetComponent<LobbyPlayer>().enabled = false;
            dogController.GetComponent<Rigidbody>().useGravity = true;

            OnDogSpawned?.Invoke(dogController.transform);
        }
    }
}