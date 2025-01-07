using UnityEngine;
using UnityEngine.UI;

public class HoldButtonTask : Task
{
    [SerializeField] Slider slider;
    [SerializeField] float holdDownTime = 3f;
    [HideInInspector] public float interactSpeedMultiplier = 1f;

    float currentTime;

    protected override void Start()
    {
        base.Start();
    }

    protected override void UpdateLogic()
    {
        base.UpdateLogic();

        if (isInteracting) HoldingDown();

        slider.value = currentTime / holdDownTime;

        if (currentTime >= holdDownTime)
        {
            EndTask();
        }
    }

    public void HoldingDown() // call every frame
    {
        currentTime += interactSpeedMultiplier * Time.deltaTime;
    }

    public override void StartTask(Interactable interactable)
    {
        base.StartTask(interactable);

        currentTime = 0f;
        slider.value = 0f;
    }
}
