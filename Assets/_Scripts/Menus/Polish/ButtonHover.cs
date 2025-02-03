using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{


    [SerializeField] bool playSoundOnHover = true;
    [SerializeField] float audioPitchMin = 0.9f, audioPitchMax = 1.1f;

    static bool isSelecting;

    float noInstantSoundTimer = 0.1f;

    protected virtual void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
    }
    void Update()
    {
        noInstantSoundTimer -= Time.deltaTime;
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
        if (playSoundOnHover && noInstantSoundTimer < 0) SoundManager.Instance.PlaySoundWithRandomPitch(SoundManager.Instance.uiSFX.buttonHoverSound, null, audioPitchMin, audioPitchMax);
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
        if (isSelecting) yield break; // to not be able to call this multiple times in a frame, which causes problems

        isSelecting = true;
        yield return null; // Wait until the end of the frame
        EventSystem.current.SetSelectedGameObject(gameObject);
        isSelecting = false;
    }
}