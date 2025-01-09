using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

public class TestDogManager : MonoBehaviour
{
    PlayerInputManager playerInputManager;

    public event Action<Transform> OnDogSpawned;
    public event Action<Transform> OnDogDespawned;

    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerInputManager.onPlayerJoined += OnPlayerJoined;
        playerInputManager.onPlayerLeft += OnPlayerLeft;
    }

    private void OnPlayerJoined(PlayerInput input)
    {
        OnDogSpawned?.Invoke(input.transform);
    }

    private void OnPlayerLeft(PlayerInput input)
    {
        OnDogDespawned?.Invoke(input.transform);
    }
}
