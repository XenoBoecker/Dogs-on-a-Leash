using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ButtonHover))]
public class ButtonHoverColorChange : MonoBehaviour
{
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color notHoverColor;

    // Start is called before the first frame update
    void Start()
    {
        ButtonHover buttonHover = GetComponent<ButtonHover>();
        if(buttonHover.IsHovering)
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
        GetComponent<Image>().color = hoverColor;
    }

    void SetNotHovering()
    {
        GetComponent<Image>().color = notHoverColor;
    }
}
