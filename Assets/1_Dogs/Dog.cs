using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dog : MonoBehaviour
{
    public DogData DogData;
    public int ColorIndex;
    public int AccessorieIndex;

    public event Action OnDogDataChanged;
    public void SetDogData(DogData dogData)
    {
        DogData = dogData;

        OnDogDataChanged?.Invoke();
    }

    internal void SetAccessorieIndex(int accessorieIndex)
    {
        Debug.Log("Dog SetACCIndex: " + accessorieIndex);

        AccessorieIndex = accessorieIndex;

        OnDogDataChanged?.Invoke();
    }

    internal void SetColor(int colorIndex)
    {
        ColorIndex = colorIndex;

        OnDogDataChanged?.Invoke();
    }

    internal void SetPlayerInput(PlayerInput playerInput)
    {
        GetComponent<PlayerDogController>().SetPlayerInput(playerInput);
    }
}
