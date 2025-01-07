using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDetector : MonoBehaviour
{
    PlayerDogController playerDogController;
    
    private List<IInteractable> _interactablesInRange = new List<IInteractable>();

    IInteractable currentClosestInteractable;

    IInteractable currentInteractingInteractable;
    public IInteractable CurrentInteractingInteractable => currentInteractingInteractable;

    public delegate void OnInteracted();
    public OnInteracted onInteracted;

    public event Action OnInteractEnded;

    private void Start()
    {
        playerDogController = GetComponent<PlayerDogController>();
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
        IInteractable closestInteractable = GetClosestInteractable();

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


    private void OnTriggerEnter2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();

        if (interactable == null) return;

        _interactablesInRange.Add(interactable);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();

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

    IInteractable GetClosestInteractable()
    {
        float closestDistance = float.MaxValue;

        IInteractable closestInteractable = null;

        foreach (IInteractable interactable in _interactablesInRange)
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
    private bool IsVisible(IInteractable interactable)
    {
        // cast ray to interactable

        Vector2 direction = interactable.MyTransform.position - transform.position;

        // LayerMask wallLayerMask = LayerMask.GetMask("Wall");

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Vector3.Magnitude(direction));

        hitPoint = hit.point;
        dir = direction;
        mag = Vector3.Magnitude(direction);

        if (hit.collider == null) return false;

        if (hit.collider.GetComponent<IInteractable>() == null) return false;

        if (hit.collider.GetComponent<IInteractable>() != interactable) return false;

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
