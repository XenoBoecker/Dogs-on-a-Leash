using System.Collections.Generic;
using UnityEngine;

public class LobbyData : MonoBehaviour
{
    [SerializeField] DogData[] availableDogs;
    public DogData[] AvailableDogs => availableDogs;


    [SerializeField] DogVisuals dogVisualsPrefab;

    public int AvailableAccessoriesCount => dogVisualsPrefab.AccessorieCount;
}