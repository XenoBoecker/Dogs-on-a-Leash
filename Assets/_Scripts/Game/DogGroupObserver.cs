using System.Collections.Generic;
using UnityEngine;

public class DogGroupObserver : MonoBehaviour
{
    DogManager dogManager;

    public List<Transform> allDogs = new List<Transform>();

    Vector3 avgDogPosition;
    public Vector3 AvgDogPosition => avgDogPosition;
    Vector3 avgDogDirection;
    public Vector3 AvgDogDirection => avgDogDirection;

    private void Start()
    {
        InvokeRepeating(nameof(UpdateGroupData), 0.5f, 0.2f);
    }

    void UpdateGroupData()
    {
        avgDogPosition = Vector3.zero;
        avgDogDirection = Vector3.zero;

        for (int i = 0; i < allDogs.Count; i++)
        {
            avgDogPosition += allDogs[i].position;
            avgDogDirection += allDogs[i].GetComponent<Rigidbody>().velocity;
        }

        avgDogPosition /= allDogs.Count;
        avgDogDirection /= allDogs.Count;
    }

    private void OnEnable()
    {
        dogManager = FindObjectOfType<DogManager>();
        dogManager.OnDogSpawned += AddDogToGroup;
    }

    private void AddDogToGroup(Transform dog)
    {
        allDogs.Add(dog);
    }
}