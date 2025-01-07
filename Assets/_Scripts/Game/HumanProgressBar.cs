using UnityEngine;
using UnityEngine.UI;

public class HumanProgressBar : MonoBehaviour
{
    HumanMovement human;
    MapManager mapGen;


    [SerializeField] Slider slider;

    private void Awake()
    {
        human = FindObjectOfType<HumanMovement>();
        mapGen = FindObjectOfType<MapManager>();
    }

    private void Update()
    {
        float progressPercentage = human.transform.position.x / mapGen.levelLength;

        slider.value = progressPercentage;
    }
}