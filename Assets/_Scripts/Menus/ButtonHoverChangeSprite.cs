using UnityEngine;
using UnityEngine.UI;

public class ButtonHoverChangeSprite : ButtonHover
{
    [SerializeField] Image buttonImage;
    [SerializeField] Sprite hoverSprite;
    [SerializeField] Sprite normalSprite;



    public override void OnHoverEnter()
    {
        base.OnHoverEnter();

        buttonImage.sprite = hoverSprite;
    }

    public override void OnHoverExit()
    {
        buttonImage.sprite = normalSprite;
    }
}