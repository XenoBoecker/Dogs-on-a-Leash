using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GroupCameraManager : MonoBehaviour
{
    DogManager dogManager;
    TestDogManager testDogManager;

    [SerializeField] Transform human;
    [SerializeField] CinemachineTargetGroup cinemachineTargetGroup;

    List<CinemachineTargetGroup.Target> dogTargets = new List<CinemachineTargetGroup.Target>();


    private void Awake()
    {
        dogManager = GetComponent<DogManager>();
        if (dogManager != null)
        {
            dogManager.OnDogSpawned += AddTarget;
            dogManager.OnDogDespawned += RemoveTarget;
        }

        TestAwake();
    }

    private void TestAwake()
    {
        testDogManager = GetComponent<TestDogManager>();
        if (testDogManager != null)
        {
            testDogManager.OnDogSpawned += AddTarget;
            testDogManager.OnDogDespawned += RemoveTarget;
        }
    }

    private void Start()
    {
        AddTarget(human);
    }

    public void AddTarget(Transform dogTransform)
    {
        CinemachineTargetGroup.Target dogTarget = new CinemachineTargetGroup.Target();

        dogTarget.target = dogTransform;
        dogTarget.weight = 1;
        dogTarget.radius = 1;

        dogTargets.Add(dogTarget);

        UpdateCameraTargets();
    }

    public void RemoveTarget(Transform dogTransform)
    {
        foreach (CinemachineTargetGroup.Target dogTarget in dogTargets)
        {
            if (dogTarget.target == dogTransform) dogTargets.Remove(dogTarget);
        }

        UpdateCameraTargets();
    }

    private void UpdateCameraTargets()
    {
        cinemachineTargetGroup.m_Targets = new CinemachineTargetGroup.Target[dogTargets.Count];

        for (int i = 0; i < dogTargets.Count; i++)
        {
            cinemachineTargetGroup.m_Targets[i] = dogTargets[i];
        }
    }
}