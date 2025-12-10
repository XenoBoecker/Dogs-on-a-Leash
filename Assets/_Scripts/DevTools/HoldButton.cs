using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [Tooltip("Seconds the button must be held before invoking OnHold")]
    public float holdTime = 1f;

    [Tooltip("Event invoked after hold time is reached")]
    public UnityEvent OnHold;

    bool pointerDown;
    float holdTimer;

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
        holdTimer = 0f;
        Debug.Log("Pointer Down");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        CancelHold();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CancelHold();
    }

    void Update()
    {
        if (!pointerDown) return;

        holdTimer += Time.unscaledDeltaTime; // use unscaled if you want it to ignore timeScale changes
        if (holdTimer >= holdTime)
        {
            OnHold?.Invoke();
            CancelHold(); // reset so it doesn't fire repeatedly; remove if you want repeat behavior
        }
    }

    void CancelHold()
    {
        pointerDown = false;
        holdTimer = 0f;
        Debug.Log("Pointer Up or Exit");
    }
}