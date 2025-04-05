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
    public static Action<Interactable> OnTaskCompleted;

    public List<InteractableDetector> currentInteractors = new List<InteractableDetector>();
    bool isInteracting;

    protected bool isCompleted;
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
        if (!isCompleted)
        {
            OnTaskCompleted?.Invoke(this);

            StartCoroutine(SpawnVFXDelayed());

            isCompleted = true;
        }

        EndAllInteractions();
    }

    IEnumerator SpawnVFXDelayed()
    {
        yield return new WaitForSeconds(spawnVFXTimeDelay);

        if (completeTaskVFX != null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.scoreSFX.smallGain);

            completeTaskVFX.Play();
            completeTaskVFX.transform.SetParent(null);
            Destroy(completeTaskVFX, 1f);
        }
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
