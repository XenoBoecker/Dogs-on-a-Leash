using System;
using UnityEngine;

public class DogStateObserver : MonoBehaviour
{
    PlayerDogController pdc;
    Rigidbody dogRB;
    InteractableDetector detector;

    public event Action OnStartWalking;
    public event Action OnStopWalking;
    public event Action OnBark;
    public event Action OnInteract;
    public event Action OnInteractEnd;


    bool wasWalkingLastFrame;
    float moveXInputLastFrame;

    public float CurrentSpeedPercentage => dogRB.velocity.magnitude / pdc.maxSpeed;
    public int LeanDirInt;


    [SerializeField] float leanMinAngle = 5f;


    private void Start()
    {
        pdc = GetComponent<PlayerDogController>();
        dogRB = GetComponent<Rigidbody>();
        detector = GetComponentInChildren<InteractableDetector>();

        pdc.OnBark += () => OnBark?.Invoke();
        pdc.OnInteract += OnInteract;

        detector.onInteracted += InteractStarted;
        detector.OnInteractEnded += InteractEnded;
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

        Vector3 input = new Vector3(pdc.MovementInput.x, 0, pdc.MovementInput.y);
        // Calculate the signed angle
        float angleDifference = Vector3.SignedAngle(transform.forward, input, Vector3.up);

        // Set leanDirInt based on the angleDifference
        if (Mathf.Abs(angleDifference) < leanMinAngle)
        {
            LeanDirInt = 0; // No significant rotation
        }
        else
        {
            LeanDirInt = angleDifference > 0 ? 1 : -1; // Right if positive, left if negative
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

    private void InteractStarted()
    {
        throw new NotImplementedException();
    }

    private void InteractEnded()
    {
        throw new NotImplementedException();
    }
}