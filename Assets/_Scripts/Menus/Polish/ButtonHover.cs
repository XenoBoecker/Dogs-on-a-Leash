using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{


    [SerializeField] bool playSoundOnHover = true;
    [SerializeField] float audioPitchMin = 0.9f, audioPitchMax = 1.1f;

    protected virtual void Start()
    {

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
        EventSystem.current.SetSelectedGameObject(gameObject);

        if(playSoundOnHover) SoundManager.Instance.PlaySoundWithRandomPitch(SoundManager.Instance.uiSFX.buttonHoverSound, null, audioPitchMin, audioPitchMax);
    }

    public virtual void OnHoverExit()
    {
        EventSystem.current.SetSelectedGameObject(null);
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
