﻿using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Transform human;


    [SerializeField] float lerpSpeed = 2f;

    private void Awake()
    {
        human = FindObjectOfType<HumanMovement>().transform;
    }

    private void LateUpdate()
    {
        float lerpedX = Mathf.Lerp(transform.position.x, human.position.x, Time.deltaTime * lerpSpeed);

        transform.position = new Vector3(lerpedX, transform.position.y, transform.position.z);
    }
}