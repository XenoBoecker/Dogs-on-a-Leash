using System;
using UnityEngine;

public class HumanConnectionManager : MonoBehaviour
{
    OnlineDogSpawner dogSpawner;

    TestDogManager testDogManager;


    Transform human;


    [SerializeField] bool testingNoDogAttachment;

    private void Awake()
    {
        human = FindObjectOfType<HumanMovement>().transform;

        dogSpawner = FindObjectOfType<OnlineDogSpawner>();
        if (dogSpawner != null)
        {
            dogSpawner.OnDogSpawned += AttachDogToHuman;
        }

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
        if (testingNoDogAttachment)
        {
            Destroy(dogTransform.GetComponent<SpringJoint>());
            return;
        }

        dogTransform.GetComponent<SpringJoint>().connectedBody = human.GetComponent<Rigidbody>();
    }
}