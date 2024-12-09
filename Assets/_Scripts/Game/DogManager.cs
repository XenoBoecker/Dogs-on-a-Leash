using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DogManager : MonoBehaviour
{

    [SerializeField] AIDogController aiDogPrefab;

    [SerializeField] DogData[] allDogDatas; // will later just be taken from whats chosen in the lobby

    public event Action<Transform> OnDogSpawned;
    public event Action<Transform> OnDogDespawned;


    [SerializeField] int totalDogCount = 8;
    int playerCount;

    private void Start()
    {
        DogController[] dogControllers = FindObjectsOfType<DogController>(true);
        playerCount = dogControllers.Length;

        foreach (DogController dogController in dogControllers)
        {
            dogController.gameObject.SetActive(true);
            dogController.GetComponentInParent<PlayerInput>().currentActionMap = dogController.GetComponentInParent<PlayerInput>().actions.FindActionMap("Player");
            dogController.enabled = true;
            dogController.GetComponentInParent<LobbyPlayer>().enabled = false;

            dogController.GetComponent<Dog>().SetDogData(allDogDatas[Array.IndexOf(dogControllers, dogController)]);

            OnDogSpawned?.Invoke(dogController.transform);
        }

        SpawnAIDogs();
    }

    private void SpawnAIDogs()
    {
        for (int i = 0; i < totalDogCount - playerCount; i++)
        {
            AIDogController aiDog = Instantiate(aiDogPrefab);

            aiDog.GetComponent<Dog>().SetDogData(allDogDatas[playerCount + i]);

            OnDogSpawned?.Invoke(aiDog.transform);
        }
    }
}