using UnityEngine;

public class ButtonHoverBobbing : ButtonHover
{
    [SerializeField] private Transform buttonObject;

    [SerializeField] private float bobSpeed = 3;

    [SerializeField] private float bobScaleFactor = 1.05f;

    [SerializeField] private float returnSpeed = 5f; // Speed for smooth transition back to normal scale


    [SerializeField] bool onlyOnce;

    [ConditionalHide("onlyOnce")]

    [SerializeField] AnimationCurve sizeIncreaseAnimCurve;

    [ConditionalHide("onlyOnce")]

    [SerializeField] float animDuration = 0.2f;



    private float startTime;
    Vector3 startScale;

    private bool isBobbing;
    private float currentAnimTime;
    private float currentScale = 1f;

    protected override void Start()
    {
        base.Start();

        if (buttonObject == null) buttonObject = transform;
        startScale = buttonObject.transform.localScale;
    }

    private void Update()
    {
        if (isBobbing)
        {
            // Handle bobbing animation
            currentAnimTime = Time.time - startTime; // Use Time.time instead of Time.deltaTime

            if (onlyOnce) currentScale = 1 + sizeIncreaseAnimCurve.Evaluate(currentAnimTime / animDuration) * (bobScaleFactor-1);
            else currentScale = 1 + Mathf.Sin(currentAnimTime * bobSpeed) * (bobScaleFactor - 1);
        }
        else
        {
            // Smoothly return to normal scale when not bobbing
            currentScale = Mathf.Lerp(currentScale, 1f, Time.deltaTime * returnSpeed);
        }

        buttonObject.transform.localScale = startScale * currentScale;
    }

    public override void OnHoverEnter()
    {
        base.OnHoverEnter();

        isBobbing = true;
        startTime = Time.time; // Reset start time on hover enter
    }

    public override void OnHoverExit()
    {
        isBobbing = false;
    }
}