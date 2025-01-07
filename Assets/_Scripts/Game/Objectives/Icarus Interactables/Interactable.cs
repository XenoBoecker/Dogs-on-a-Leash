using System;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    HoldButtonTask repairTask;
    
    [SerializeField] Task task;

    [SerializeField] GameObject showInteractable;

    public Transform MyTransform { get; set; }
    public Action OnInteractEnd { get; set; }


    bool isInteracting;
    public void SetIsInteracting(bool value) 
    {
        isInteracting = value;
    }


    protected virtual void Awake()
    {
        repairTask = FindObjectOfType<HoldButtonTask>();
        MyTransform = transform;
    }

    private void Start()
    {
        repairTask.OnInteractEnd += InteractEnd;
        if(task != null) task.OnInteractEnd += InteractEnd;

        HideInteractable();
    }

    public void Interact()
    {
        if (isInteracting) return;

        SetIsInteracting(true);


        if (task == null)
        {
            InteractEnd();
            return;
        }

        task.StartTask(this);
    }

    public void InteractEnd()
    {
        if (!isInteracting) return;

        SetIsInteracting(false);
        
        OnInteractEnd?.Invoke();
    }

    public void ShowInteractable()
    {
        showInteractable.SetActive(true);
    }

    public void HideInteractable()
    {
        showInteractable.SetActive(false);
    }

    internal virtual void CompleteTask()
    {
        InteractEnd();
    }

    public void CancelTask()
    {
        repairTask.EndTask();
        if(task != null) task.EndTask();
    }
}
