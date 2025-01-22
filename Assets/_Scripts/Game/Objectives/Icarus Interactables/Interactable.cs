using System;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{    
    Task task;
    public Task Task => task;

    [SerializeField] GameObject showInteractable;


    [SerializeField] bool singleDogInteractable = true;

    public Transform MyTransform { get; set; }
    public Action OnInteractEnd { get; set; }

    public List<InteractableDetector> currentInteractors = new List<InteractableDetector>();
    bool isInteracting;
    public void SetIsInteracting(bool value) 
    {
        isInteracting = value;
    }


    protected virtual void Awake()
    {
        MyTransform = transform;
        task = GetComponent<Task>();
    }

    private void Start()
    {
        if(task != null) task.OnInteractEnd += EndAllInteractions;

        HideInteractable();
    }

    public void Interact(InteractableDetector interactor)
    {
        Debug.Log("Current Interactors Count: " + currentInteractors.Count);

        if (isInteracting && singleDogInteractable) return;

        currentInteractors.Add(interactor);

        SetIsInteracting(true);


        if (task == null)
        {
            CompleteTask();
            return;
        }

        if(currentInteractors.Count == 1) task.StartTask(this);
    }

    void EndAllInteractions()
    {
        currentInteractors.Clear();

        SetIsInteracting(false);

        OnInteractEnd?.Invoke();
    }

    public void InteractEnd(InteractableDetector interactor)
    {
        Debug.Log("InteractEnd, currentCount: "+currentInteractors.Count);

        if (!isInteracting) return;

        if (currentInteractors.Count == 0)
        {
            SetIsInteracting(false);

        }

        OnInteractEnd?.Invoke();
    }

    public void ShowInteractable()
    {
        if (showInteractable != null) showInteractable.SetActive(true);
    }

    public void HideInteractable()
    {
        if(showInteractable != null) showInteractable.SetActive(false);
    }

    internal virtual void CompleteTask()
    {
        EndAllInteractions();
    }

    public void CancelTask(InteractableDetector interactor)
    {
        Debug.Log("Remove " + interactor.name);

        currentInteractors.Remove(interactor);

        if (currentInteractors.Count == 0)
        {
            if (task != null) task.EndTask();
        }
        else
        {
            InteractEnd(interactor);
        }
    }
}
