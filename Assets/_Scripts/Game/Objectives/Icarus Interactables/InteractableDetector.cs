using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class InteractableDetector : MonoBehaviour
{
    PhotonView view;

    PlayerDogController playerDogController;
    Rigidbody dogRB;

    public static int PickupCount;

    private List<Interactable> _interactablesInRange = new List<Interactable>();

    Interactable currentClosestInteractable;

    Interactable currentInteractingInteractable;
    public Interactable CurrentInteractingInteractable => currentInteractingInteractable;


    [SerializeField] bool rotateTowardsInteractable = false;
    [SerializeField] float rotationTowardsInteractableSpeed;

    [SerializeField] float stayAtInteractableForce = 30f;


    [SerializeField] VisualEffect[] buddelVFXs;

    public event Action OnInteract;
    public event Action OnInteracted;

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
        playerDogController.OnStopInteract += CancelTask;

        dogRB = playerDogController.GetComponent<Rigidbody>();

        for (int i = 0; i < buddelVFXs.Length; i++)
        {
            buddelVFXs[i].gameObject.SetActive(false);
            buddelVFXs[i].Stop();
        }

        PickupCount = 0;
    }

    private void Update()
    {
        ShowClosestInteractable();

        if(currentInteractingInteractable != null)
        {
            //freeze y rotation of dog
            dogRB.constraints |= RigidbodyConstraints.FreezeRotationY; 
            RotateTowardsAndStayAtInteractable();
        }
        else
        {
            dogRB.constraints &= ~RigidbodyConstraints.FreezeRotationY; 
        }
    }
    private void RotateTowardsAndStayAtInteractable()
    {
        if (currentInteractingInteractable == null) return; // Ensure there's a target to look at

        // Get the direction to the interactable
        Vector3 direction = (currentInteractingInteractable.transform.position - playerDogController.transform.position).normalized;
        if (rotateTowardsInteractable)
        {
            // Calculate the target rotation
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

            // Smoothly interpolate the rotation
            playerDogController.transform.rotation = Quaternion.Slerp(
                playerDogController.transform.rotation,
                targetRotation,
                Time.deltaTime * rotationTowardsInteractableSpeed // Adjust rotationSpeed for desired smoothness
            );
        }

        dogRB.AddForce(direction * stayAtInteractableForce);
    }

    void CancelTask() // usually task is canceled from within task, this is for external canceling: e.g. being dragged away too far
    {
        if (currentInteractingInteractable != null) currentInteractingInteractable.CancelTask(this);
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
        // Debug.Log("Trigger enter: " + other.name);
        Interactable interactable = other.GetComponent<Interactable>();

        if (interactable == null) return;

        if (_interactablesInRange.Contains(interactable)) return;

         _interactablesInRange.Add(interactable);

        // if (currentInteractingInteractable != null) Debug.Log("has interaction a lreaty");

        if (currentInteractingInteractable == null && interactable.Task == null)
        {
            // Debug.Log("Interact automatically");
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
        OnInteract?.Invoke();

        currentInteractingInteractable = interactable;
        currentInteractingInteractable.OnInteractEnd += EndCurrentInteraction;

        if (playerDogController == null)
        {
            Debug.Log("Again find PlayerDogController");
            playerDogController = GetComponentInParent<PlayerDogController>();
        }

        playerDogController.StopMovement();

        currentInteractingInteractable.Interact(this);


        // only call if the interaction has not already ended inside the Interact(), because there is no Task (then EndInteract would be called before onInteracted)
        if (currentInteractingInteractable != null)
        {
            if (currentInteractingInteractable.Task is HoldButtonTask)
            {
                for (int i = 0; i < buddelVFXs.Length; i++)
                {
                    buddelVFXs[i].gameObject.SetActive(true);
                    buddelVFXs[i].Play();
                }
            }

            OnInteracted?.Invoke();
        }
    }

    void InteractWithClosestInteractable()
    {
        if (currentClosestInteractable == null) return;

        StartInteraction(currentClosestInteractable);
    }

    public void EndCurrentInteraction()
    {
        if (currentInteractingInteractable.currentInteractors.Contains(this)) return;

        currentInteractingInteractable.OnInteractEnd -= EndCurrentInteraction;
        currentInteractingInteractable = null;

        playerDogController.StartMovement();

        for (int i = 0; i < buddelVFXs.Length; i++)
        {
            buddelVFXs[i].gameObject.SetActive(false);
            buddelVFXs[i].Stop();
        }

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
