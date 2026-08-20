using TMPro;
using UnityEngine;

public class ButtonHoverTextColorChange : MonoBehaviour
{
    [SerializeField] private ButtonHover buttonHover;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color notHoverColor;

    // Start is called before the first frame update
    void Start()
    {
        if (buttonHover == null) buttonHover = GetComponent<ButtonHover>();
        if (buttonHover.IsHovering)
        {
            SetHovering();
        }
        else
        {
            SetNotHovering();
        }
        buttonHover.OnHoverEntered += SetHovering;
        buttonHover.OnHoverExited += SetNotHovering;
    }

    void SetHovering()
    {
        GetComponent<TMP_Text>().color = hoverColor;
    }

    void SetNotHovering()
    {
        GetComponent<TMP_Text>().color = notHoverColor;
    }
}
