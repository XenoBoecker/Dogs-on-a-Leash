using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderSelectedBGChange : ButtonHover
{

    [SerializeField] Image bgImage;

    [SerializeField] Sprite baseSprite, selectedSprite;

    public override void OnHoverEnter()
    {
        base.OnHoverEnter();

        bgImage.sprite = selectedSprite;
    }

    public override void OnHoverExit()
    {
        base.OnHoverExit();

        bgImage.sprite = baseSprite;
    }
}
