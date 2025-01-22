using UnityEngine;
using UnityEngine.UI;

public class HoldButtonTask : Task
{
    [SerializeField] Image fillImage;
    [SerializeField] float holdDownTime = 3f;
    [HideInInspector] public float interactSpeedMultiplier = 1f;

    float currentTime;

    protected override void UpdateLogic()
    {
        base.UpdateLogic();

        if (isInteracting) HoldingDown();

        fillImage.fillAmount = currentTime / holdDownTime;

        if (currentTime >= holdDownTime)
        {
            CompleteTask();
            EndTask();
        }
    }

    public void HoldingDown() // call every frame
    {
        currentTime += interactSpeedMultiplier * Time.deltaTime * interactable.currentInteractors.Count;
    }

    public override void StartTask(Interactable interactable)
    {
        base.StartTask(interactable);

        currentTime = 0f;
        fillImage.fillAmount = 0f;
    }
}
