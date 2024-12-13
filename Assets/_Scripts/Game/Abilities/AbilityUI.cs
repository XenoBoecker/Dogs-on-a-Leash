using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{

    [SerializeField] Canvas canvas;
    [SerializeField] Image image;
    [SerializeField] Ability ability;

    private void Update()
    {
        // Update the fill amount based on ability charge
        image.fillAmount = ability.CurrentChargePercentage();

        canvas.transform.LookAt(Camera.main.transform);
    }
}