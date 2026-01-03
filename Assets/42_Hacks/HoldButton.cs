using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoldButton : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerExitHandler,
    ISelectHandler,
    IDeselectHandler,
    ISubmitHandler
{
    [Tooltip("Seconds the button must be held before invoking OnHold")]
    public float holdTime = 1f;

    [Tooltip("Event invoked after hold time is reached")]
    public UnityEvent OnHold;

    [SerializeField] private Image holdButtonFillImage;

    bool pointerDown;
    bool mousePointerDown;
    bool isSelected;
    float holdTimer;

    void Update()
    {
        if (holdButtonFillImage != null)
        {
            holdButtonFillImage.fillAmount = holdTimer / holdTime;
        }

        if (!pointerDown)
            return;

        // Cancel when submit is released (keyboard / controller)
        if (isSelected && !Input.GetButton("Submit") && !mousePointerDown)
        {
            CancelHold();
            return;
        }

        holdTimer += Time.unscaledDeltaTime;

        if (holdTimer >= holdTime)
        {
            OnHold?.Invoke();
            CancelHold();
        }
    }

    // =====================
    // Pointer (Mouse / Touch)
    // =====================

    public void OnPointerDown(PointerEventData eventData)
    {
        mousePointerDown = true;
        StartHold();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        mousePointerDown = false;
        CancelHold();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mousePointerDown = false;
        CancelHold();
    }

    // =====================
    // Keyboard / Controller
    // =====================

    public void OnSelect(BaseEventData eventData)
    {
        isSelected = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isSelected = false;
        CancelHold();
    }

    // Called when Submit (Enter / A / Cross) is pressed
    public void OnSubmit(BaseEventData eventData)
    {
        if (!pointerDown && isSelected)
        {
            StartHold();
        }
    }

    // =====================
    // Helpers
    // =====================

    void StartHold()
    {
        pointerDown = true;
        holdTimer = 0f;
    }

    void CancelHold()
    {
        pointerDown = false;
        holdTimer = 0f;
    }
}
