using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;
/*
public class GroupCameraManager : MonoBehaviour
{
    DogManager dogManager;
    TestDogManager testDogManager;

    DogGroupObserver dogGroupObserver;

    Transform human;
    [SerializeField] CinemachineTargetGroup cinemachineTargetGroup;


    [SerializeField] float rotSmoothFactor = 1f;

    List<CinemachineTargetGroup.Target> dogTargets = new List<CinemachineTargetGroup.Target>();


    private void Awake()
    {
        human = FindObjectOfType<HumanMovement>().transform;

        dogManager = GetComponent<DogManager>();
        if (dogManager != null)
        {
            dogManager.OnDogSpawned += AddTarget;
            dogManager.OnDogDespawned += RemoveTarget;
        }

        dogGroupObserver = FindObjectOfType<DogGroupObserver>();

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

    private void Update()
    {
        // RotateCameraWithGroupDirection(); // inputs need to rotate aswell if we do this
    }

    private void RotateCameraWithGroupDirection()
    {

        // Convert 2D direction back to 3D
        Vector3 targetDirection = new Vector3(dogGroupObserver.AvgDogDirection.x, 0, dogGroupObserver.AvgDogDirection.z);

        // Smoothly rotate the Cinemachine camera towards the movement direction
        if (targetDirection.sqrMagnitude > 0.01f) // Avoid unnecessary rotation when the direction is nearly zero
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            cinemachineTargetGroup.transform.rotation = Quaternion.Slerp(
                cinemachineTargetGroup.transform.rotation,
                targetRotation,
                Time.deltaTime * rotSmoothFactor // Smooth factor, tweak as needed
            );
        }
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
}*/