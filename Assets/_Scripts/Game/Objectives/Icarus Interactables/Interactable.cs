using System;
using UnityEngine;

public class Interactable : MonoBehaviour
{    
    Task task;
    public Task Task => task;

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
        MyTransform = transform;
        task = GetComponent<Task>();
    }

    private void Start()
    {
        if(task != null) task.OnInteractEnd += InteractEnd;

        HideInteractable();
    }

    public void Interact()
    {
        if (isInteracting) return;

        SetIsInteracting(true);


        if (task == null)
        {
            CompleteTask();
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
        if (showInteractable != null) showInteractable.SetActive(true);
    }

    public void HideInteractable()
    {
        if(showInteractable != null) showInteractable.SetActive(false);
    }

    internal virtual void CompleteTask()
    {
        InteractEnd();
    }

    public void CancelTask()
    {
        if(task != null) task.EndTask();
    }
}
