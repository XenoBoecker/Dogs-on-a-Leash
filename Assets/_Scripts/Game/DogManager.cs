using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DogManager : MonoBehaviour
{

    [SerializeField] GameObject aiDogPrefab;

    public event Action<Transform> OnDogSpawned;
    public event Action<Transform> OnDogDespawned;

    private void Start()
    {
        DogController[] dogControllers = FindObjectsOfType<DogController>(true);
        
        foreach (DogController dogController in dogControllers)
        {
            dogController.gameObject.SetActive(true);
            dogController.GetComponentInParent<PlayerInput>().currentActionMap = dogController.GetComponentInParent<PlayerInput>().actions.FindActionMap("Player");
            dogController.enabled = true;
            dogController.GetComponentInParent<LobbyPlayer>().enabled = false;
            dogController.GetComponent<Rigidbody>().useGravity = true;

            OnDogSpawned?.Invoke(dogController.transform);
        }

        SpawnAIDogs();
    }

    private void SpawnAIDogs()
    {
        
    }
}