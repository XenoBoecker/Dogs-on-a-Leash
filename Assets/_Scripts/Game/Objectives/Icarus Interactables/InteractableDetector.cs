using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDetector : MonoBehaviour
{
    PlayerDogController playerDogController;
    
    private List<Interactable> _interactablesInRange = new List<Interactable>();

    Interactable currentClosestInteractable;

    Interactable currentInteractingInteractable;
    public Interactable CurrentInteractingInteractable => currentInteractingInteractable;

    public delegate void OnInteracted();
    public OnInteracted onInteracted;

    public event Action OnInteractEnded;

    private void Start()
    {
        playerDogController = GetComponentInParent<PlayerDogController>();
        playerDogController.OnInteract += OnInteract;
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
            interactable.Interact(); // auto interact if no task todo 
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

    void OnInteract()
    {
        if (currentClosestInteractable == null) return;

        currentInteractingInteractable = currentClosestInteractable;
        currentInteractingInteractable.OnInteractEnd += OnInteractEnd;
        playerDogController.StopMovement();
        currentInteractingInteractable.Interact();

        if(currentInteractingInteractable != null) onInteracted?.Invoke(); // only call if the interaction has not already ended inside the Interact(), because there is no Task (then EndInteract would be called before onInteracted)
    }

    public void OnInteractEnd()
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
            if (!IsVisible(interactable)) continue;

            float distance = Vector2.Distance(transform.position, interactable.MyTransform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestInteractable = interactable;
            }
        }

        return closestInteractable;
    }

    Vector3 hitPoint;
    Vector3 dir;
    float mag;
    private bool IsVisible(Interactable interactable)
    {
        // cast ray to interactable

        Vector2 direction = interactable.MyTransform.position - transform.position;

        // LayerMask wallLayerMask = LayerMask.GetMask("Wall");

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Vector3.Magnitude(direction));

        hitPoint = hit.point;
        dir = direction;
        mag = Vector3.Magnitude(direction);

        if (hit.collider == null) return false;

        if (hit.collider.GetComponent<Interactable>() == null) return false;

        if (hit.collider.GetComponent<Interactable>() != interactable) return false;

        return true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + dir.normalized * mag);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, hitPoint);
    }

    // internal void InteractInput()
    // {
    //     print("DoInteract");
    // }
}
