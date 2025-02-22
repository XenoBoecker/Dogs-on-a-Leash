﻿using UnityEngine;

public class WobbleUpAndDown : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f, moveScale = 0.5f;
    [SerializeField] private float rotSpeed = 2f, rotScale = 15f;

    private float timeOffset;

    float startY;
    float startRot;

    private void Start()
    {
        // Offset to ensure different objects wobble at different starting points
        timeOffset = Random.Range(0f, Mathf.PI * 2);

        startY = transform.localPosition.y;
        startRot = transform.localRotation.z;
    }

    private void Update()
    {
        float wobble = Mathf.Sin(Time.time * moveSpeed + timeOffset);

        // Move up and down
        transform.localPosition = new Vector3(transform.localPosition.x, startY + wobble * moveScale, transform.localPosition.z);

        // Rotate left and right
        transform.localRotation = Quaternion.Euler(0, 0, startRot + wobble * rotScale);
    }
}
