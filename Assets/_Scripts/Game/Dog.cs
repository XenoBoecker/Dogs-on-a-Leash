using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dog : MonoBehaviour
{
    public DogData DogData;

    public event Action OnDogDataChanged;
    public void SetDogData(DogData dogData)
    {
        DogData = dogData;

        OnDogDataChanged?.Invoke();
    }

    internal void SetPlayerInput(PlayerInput playerInput)
    {
        GetComponent<PlayerDogController>().SetPlayerInput(playerInput);
    }
}
