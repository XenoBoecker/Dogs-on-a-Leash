using System;
using UnityEngine;

public class HumanConnectionManager : MonoBehaviour
{
    DogManager dogManager;

    TestDogManager testDogManager;


    [SerializeField] Transform human;

    private void Awake()
    {
        dogManager = GetComponent<DogManager>();
        if(dogManager != null) dogManager.OnDogSpawned += AttachDogToHuman;

        TestAwake();
    }

    private void TestAwake()
    {
        testDogManager = GetComponent<TestDogManager>();
        if (testDogManager != null)
        {
            testDogManager.OnDogSpawned += AttachDogToHuman;
        }
    }

    private void AttachDogToHuman(Transform dogTransform)
    {
        dogTransform.GetComponent<SpringJoint>().connectedBody = human.GetComponent<Rigidbody>();
    }
}