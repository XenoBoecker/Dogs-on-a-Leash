using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{


    [SerializeField] bool playSoundOnHover = true;
    [SerializeField] float audioPitchMin = 0.9f, audioPitchMax = 1.1f;

    static bool isSelecting;

    static Coroutine setSelectedCoroutine;

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
        // if (setSelectedCoroutine != null) StopCoroutine(setSelectedCoroutine); // does not work for some reason
        StartCoroutine(SetSelectedAfterFrame());
        if (playSoundOnHover) SoundManager.Instance.PlaySoundWithRandomPitch(SoundManager.Instance.uiSFX.buttonHoverSound, null, audioPitchMin, audioPitchMax);
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
        Debug.Log("OnButtonClick");
        SoundManager.Instance.PlaySound(SoundManager.Instance.uiSFX.buttonClickSound);
    }
    private System.Collections.IEnumerator SetSelectedAfterFrame()
    {
        if (isSelecting) yield break;

        isSelecting = true;
        yield return null; // Wait until the end of the frame
        EventSystem.current.SetSelectedGameObject(gameObject);
        isSelecting = false;
    }
}