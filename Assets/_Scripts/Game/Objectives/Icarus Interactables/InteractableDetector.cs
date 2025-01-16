using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDetector : MonoBehaviour
{
    PhotonView view;

    PlayerDogController playerDogController;

    private List<Interactable> _interactablesInRange = new List<Interactable>();

    Interactable currentClosestInteractable;

    Interactable currentInteractingInteractable;
    public Interactable CurrentInteractingInteractable => currentInteractingInteractable;

    public delegate void OnInteracted();
    public OnInteracted onInteracted;

    public event Action OnInteractEnded;

    private void Awake()
    {
        view = GetComponentInParent<PhotonView>();
        if (PhotonNetwork.IsConnected && !view.IsMine) this.enabled = false;
    }

    private void Start()
    {
        playerDogController = GetComponentInParent<PlayerDogController>();
        if (playerDogController == null) Debug.LogError("No player dog controller found");
        playerDogController.OnInteract += InteractWithClosestInteractable;
    }

    private void Update()
    {
        ShowClosestInteractable();
    }

    void CancelTask() // usually task is canceled from within task, this is for external canceling: e.g. being dragged away too far
    {
        if (currentInteractingInteractable != null) currentInteractingInteractable.CancelTask();
    }

    private void ShowClosestInteractable()
    {
        Interactable closestInteractable = GetClosestInteractable();

        if (currentClosestInteractable != closestInteractable)
        {
            if (currentClosestInteractable != null)
            {
                currentClosestInteractable.HideInteractable();
            }

            currentClosestInteractable = closestInteractable;
            if (currentClosestInteractable != null) currentClosestInteractable.ShowInteractable();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger enter: " + other.name);
        Interactable interactable = other.GetComponent<Interactable>();

        if (interactable == null) return;

        _interactablesInRange.Add(interactable);

        if (currentInteractingInteractable != null) Debug.Log("has interaction a lreaty");

        if (currentInteractingInteractable == null && interactable.Task == null)
        {
            Debug.Log("Interact automatically");
            StartInteraction(interactable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();

        if (interactable == null) return;

        interactable.HideInteractable();

        if (interactable == currentInteractingInteractable) CancelTask();

        _interactablesInRange.Remove(interactable);
    }

    void StartInteraction(Interactable interactable)
    {
        currentInteractingInteractable = interactable;
        currentInteractingInteractable.OnInteractEnd += EndCurrentInteraction;

        if (playerDogController == null)
        {
            Debug.Log("Again find PlayerDogController");
            playerDogController = GetComponentInParent<PlayerDogController>();
        }

        playerDogController.StopMovement();

        currentInteractingInteractable.Interact();
        // only call if the interaction has not already ended inside the Interact(), because there is no Task (then EndInteract would be called before onInteracted)
        if (currentInteractingInteractable != null) onInteracted?.Invoke();
    }

    void InteractWithClosestInteractable()
    {
        if (currentClosestInteractable == null) return;

        StartInteraction(currentClosestInteractable);
    }

    public void EndCurrentInteraction()
    {
        currentInteractingInteractable = null;

        playerDogController.StartMovement();

        OnInteractEnded?.Invoke();
    }

    Interactable GetClosestInteractable()
    {
        float closestDistance = float.MaxValue;

        Interactable closestInteractable = null;

        foreach (Interactable interactable in _interactablesInRange)
        {
            if (interactable == null) continue;

            float distance = Vector2.Distance(transform.position, interactable.MyTransform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestInteractable = interactable;
            }
        }

        return closestInteractable;
    }
}
