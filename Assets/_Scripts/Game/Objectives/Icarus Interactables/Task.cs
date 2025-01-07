using System;
using UnityEngine;

public abstract class Task : MonoBehaviour
{
    [SerializeField] protected GameObject showTaskStuff;

    protected Interactable interactable;

    protected bool isInteracting;
    public event Action OnInteractEnd;

    public event Action OnStartTask;

    protected virtual void Start()
    {        
        if(showTaskStuff != null) showTaskStuff.SetActive(false);
    }

    protected virtual void LateUpdate()
    {
        if (!isInteracting) return;

        UpdateLogic();
    }

    protected virtual void UpdateLogic()
    {
        
    }

    public virtual void StartTask(Interactable interactable)
    {
        if (showTaskStuff != null) showTaskStuff.SetActive(true);

        this.interactable = interactable;

        SetIsInteracting(true);
        
        OnStartTask?.Invoke();
    }

    public virtual void EndTask() // On Cancel just call this
    {
        if (!isInteracting) return;

        if (showTaskStuff != null)
        {
            showTaskStuff.SetActive(false);
        }

        OnInteractEnd?.Invoke();

        SetIsInteracting(false);
    }

    void SetIsInteracting(bool isInteracting)
    {
        this.isInteracting = isInteracting;
        if(interactable != null) interactable.SetIsInteracting(isInteracting);
    }
}
