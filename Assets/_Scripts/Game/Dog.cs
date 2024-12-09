using System;
using UnityEngine;

public class Dog : MonoBehaviour
{
    public DogData DogData;

    public event Action OnDogDataChanged;
    public void SetDogData(DogData dogData)
    {
        DogData = dogData;

        OnDogDataChanged?.Invoke();
    }
}
