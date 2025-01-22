using System;
using UnityEngine;
using UnityEngine.UI;

public class HumanProgressBar : MonoBehaviour
{
    HumanMovement human;
    MapManager mapGen;


    [SerializeField] Slider slider;

    internal void Setup()
    {
        human = FindObjectOfType<HumanMovement>();
        mapGen = FindObjectOfType<MapManager>();
    }

    private void Update()
    {
        if (!human)
        {
            Debug.Log("No human for progress bar");
            return;
        }

        float progressPercentage = human.transform.position.x / (mapGen.currentPathLength - 15);

        slider.value = progressPercentage;
    }
}