using System;
using UnityEngine;

public class DogStateObserver : MonoBehaviour
{
    PlayerDogController pdc;

    Rigidbody dogRB;

    public event Action OnLeanLeft;
    public event Action OnLeanRight;
    public event Action OnStopLeaning;
    public event Action OnStartWalking;
    public event Action OnStopWalking;
    public event Action OnBark;
    public event Action OnInteract;


    bool wasWalkingLastFrame;
    float moveXInputLastFrame;


    private void Start()
    {
        dogRB = GetComponent<Rigidbody>();

        pdc = GetComponent<PlayerDogController>();

        pdc.OnBark += () => OnBark?.Invoke();
        pdc.OnInteract += OnInteract;
    }

    private void Update()
    {
        CheckWalking();
        CheckLeaning();


        wasWalkingLastFrame = IsWalking();
        moveXInputLastFrame = dogRB.velocity.x;

    }
    private void CheckLeaning()
    {
        if (dogRB.velocity.x < 0 && moveXInputLastFrame >= 0)
        {
            OnLeanLeft?.Invoke();
        }
        else if (dogRB.velocity.x > 0 && moveXInputLastFrame <= 0)
        {
            OnLeanRight?.Invoke();
        }
        else if (dogRB.velocity.x == 0 && moveXInputLastFrame != 0)
        {
            OnStopLeaning?.Invoke();
        }
    }

    private void CheckWalking()
    {
        if (!wasWalkingLastFrame && IsWalking())
        {
            Debug.Log("start walking");
            OnStartWalking?.Invoke();
        }
        else if (wasWalkingLastFrame && !IsWalking())
        {
            OnStopWalking?.Invoke();
        }
    }

    private bool IsWalking()
    {
        return dogRB.velocity.magnitude > 0.1f;
    }
}