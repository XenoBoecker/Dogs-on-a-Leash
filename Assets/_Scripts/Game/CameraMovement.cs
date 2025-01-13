using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Transform human;


    [SerializeField] float lerpSpeed = 2f;

    internal void Setup()
    {
        human = FindObjectOfType<HumanMovement>().transform;
    }

    private void LateUpdate()
    {
        if (!human) return;

        float lerpedX = Mathf.Lerp(transform.position.x, human.position.x, Time.deltaTime * lerpSpeed);

        transform.position = new Vector3(lerpedX, transform.position.y, transform.position.z);
    }
}