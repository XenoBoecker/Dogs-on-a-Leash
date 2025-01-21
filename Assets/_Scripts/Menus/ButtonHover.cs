﻿using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{

    [SerializeField] float audioPitchMin = 0.9f, audioPitchMax = 1.1f;

    protected virtual void Start()
    {
        // Automatically assign the event handlers
        EventTrigger trigger = gameObject.GetComponent<EventTrigger>();

        if (trigger == null)
        {
            trigger = gameObject.AddComponent<EventTrigger>();
        }

        // Add PointerEnter event
        EventTrigger.Entry pointerEnter = new EventTrigger.Entry();
        pointerEnter.eventID = EventTriggerType.PointerEnter;
        pointerEnter.callback.AddListener((data) => { OnHoverEnter(); });
        trigger.triggers.Add(pointerEnter);

        // Add PointerExit event
        EventTrigger.Entry pointerExit = new EventTrigger.Entry();
        pointerExit.eventID = EventTriggerType.PointerExit;
        pointerExit.callback.AddListener((data) => { OnHoverExit(); });
        trigger.triggers.Add(pointerExit);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHoverEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnHoverExit();
    }

    public virtual void OnHoverEnter()
    {
        SoundManager.Instance.PlaySoundWithRandomPitch(SoundManager.Instance.uiSFX.buttonHoverSound, null, audioPitchMin, audioPitchMax);
    }

    public virtual void OnHoverExit()
    {
        
    }

    public void OnSelect(BaseEventData eventData)
    {
        OnHoverEnter();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        OnHoverExit();
    }

    public void OnButtonClick()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.uiSFX.buttonClickSound);
    }
}
