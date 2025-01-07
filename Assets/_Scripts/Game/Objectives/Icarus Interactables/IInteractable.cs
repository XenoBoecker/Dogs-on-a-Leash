using System;
using UnityEngine;

public interface IInteractable
{
    public Transform MyTransform {  get; set; }

    public Action OnInteractEnd { get; set; }

    public void ShowInteractable();
    public void HideInteractable();
    public void Interact();
    public void InteractEnd();
    void CancelTask();
}
