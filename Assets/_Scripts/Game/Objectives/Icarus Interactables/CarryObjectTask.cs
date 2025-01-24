using UnityEngine;

public class CarryObjectTask : Task
{

    [SerializeField] float moveSpeedMultiplier = 0.5f;

    PlayerDogController dogController;
    public override void StartTask(Interactable interactable)
    {
        Debug.Log("Carry start");
        base.StartTask(interactable);

        dogController = interactable.currentInteractors[0].GetComponentInParent<PlayerDogController>();

        dogController.StartMovement();
        dogController.SetMoveSpeedMultiplier(moveSpeedMultiplier);

        transform.SetParent(dogController.transform);
    }
    public override void EndTask()
    {
        Debug.Log("Carry end");

        base.EndTask();

        dogController.SetMoveSpeedMultiplier(1);

        transform.SetParent(null);
    }
}