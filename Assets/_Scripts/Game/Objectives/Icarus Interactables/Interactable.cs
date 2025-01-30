using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Interactable : MonoBehaviour
{    
    Task task;
    public Task Task => task;

    [SerializeField] GameObject showInteractable;


    [SerializeField] bool singleDogInteractable = true;

    public Transform MyTransform { get; set; }

    [SerializeField] VisualEffect completeTaskVFX;

    [SerializeField] protected float spawnVFXTimeDelay = 0.3f;
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

        if(completeTaskVFX != null) completeTaskVFX.Stop();

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

        Debug.Log("complete task");

        InteractableDetector.PickupCount++;

        Debug.Log("Spawn VFD Coroutine now");

        StartCoroutine(SpawnVFXDelayed());

        Debug.Log("End all Interactinos now");

        EndAllInteractions();
    }

    IEnumerator SpawnVFXDelayed()
    {
        Debug.Log("SpawnDelayed");
        yield return new WaitForSeconds(spawnVFXTimeDelay);

        Debug.Log("Wait time over");

        if (completeTaskVFX != null)
        {
            Debug.Log("SpawnNow");
            completeTaskVFX.Play();
            completeTaskVFX.transform.SetParent(null);
            Destroy(completeTaskVFX, 1f);
        }
        else Debug.Log("no vfx herer");
    }

    public void CancelTask(InteractableDetector interactor)
    {
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
