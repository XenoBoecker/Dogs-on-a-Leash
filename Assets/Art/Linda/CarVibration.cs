using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarVibration : MonoBehaviour
{
    [SerializeField] private float vibrationSpeed = 40f; // Speed of the up and down motion
    [SerializeField] private float vibrationAmount = 0.02f; // Amount of up and down motion

    private Vector3 originalPosition;

    void Start()
    {
        // Store the original position of the GameObject
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        // Apply a small up-and-down motion to the GameObject
        float vibrationOffset = Mathf.Sin(Time.time * vibrationSpeed) * vibrationAmount;
        transform.localPosition = originalPosition + new Vector3(0f, vibrationOffset, 0f);
    }
}