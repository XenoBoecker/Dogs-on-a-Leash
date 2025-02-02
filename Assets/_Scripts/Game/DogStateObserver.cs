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

    public float CurrentSpeedPercentage => dogRB.velocity.magnitude / pdc.maxSpeed;
    public int LeanDirInt;

    public bool Digging;


    [SerializeField] float leanMinAngle = 5f;


    private void Start()
    {
        pdc = GetComponent<PlayerDogController>();
        dogRB = GetComponent<Rigidbody>();
        detector = GetComponentInChildren<InteractableDetector>();

        pdc.OnBark += () => OnBark?.Invoke();
        detector.OnInteract += OnInteract;
    }

    private void Update()
    {
        CheckWalking();
        CheckLeaning();

        Digging = IsDigging();
        wasWalkingLastFrame = IsWalking();

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
            OnStartWalking?.Invoke();
        }
        else if (wasWalkingLastFrame && !IsWalking())
        {
            OnStopWalking?.Invoke();
        }
    }
    private bool IsDigging()
    {
        Interactable currentInteractable = detector.CurrentInteractingInteractable;

        if (currentInteractable == null) return false;

        // Check if the current task is of type HoldButtonTask
        return currentInteractable.Task is HoldButtonTask;
    }

    private bool IsWalking()
    {
        return dogRB.velocity.magnitude > 0.1f;
    }
}