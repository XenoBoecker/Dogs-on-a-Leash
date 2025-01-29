﻿using Photon.Pun;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class HumanMovement : MonoBehaviour
{

    [SerializeField] Vector3 debugRBVel;

    public Transform LeashAttachmentPoint;


    [SerializeField] float startWalkingDelay = 3f;
    [SerializeField] float minSpeed = 1f;

    [SerializeField] float acceleration = 2f;
    public float speed = 5f; // Speed of movement
    public float rotationSpeed = 5f; // Speed of rotation
    private Rigidbody rb; // Rigidbody for physical movement

    float stunTime;
    bool isStunned => stunTime > 0;

    public int BumpedCount;


    [SerializeField] VisualEffect bumpPointLossVFX, stunVFX;

    public event Action<Obstacle> OnHitObstacle;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody component found on this GameObject. Please attach one.");
        }

        bumpPointLossVFX.Stop();
        stunVFX.Stop();
        stunVFX.gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        startWalkingDelay -= Time.deltaTime;
        if (startWalkingDelay > 0) return;

        if (PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient) return;

        stunTime -= Time.fixedDeltaTime;
        
        rb.rotation = Quaternion.Euler(0, rb.rotation.eulerAngles.y, 0); // constrain to only y rotation

        if (isStunned) return;

        stunVFX.Stop();
        stunVFX.gameObject.SetActive(false);

        MoveForward();
    }

    private void AddSidewaysForce()
    {
        // add a force orthogonal to the current velocity
        Vector3 direction = Vector3.Cross(rb.velocity, Vector3.up).normalized;
        rb.AddForce(direction * 0.1f, ForceMode.Impulse);
    }

    private void MoveForward()
    {
        if (PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient) return;

        if (isStunned)
        {
            return;
        }

        Vector3 direction = Vector3.right;

        if (rb.velocity.x < speed)
        {
            rb.AddForce(acceleration * direction);
        }

        debugRBVel = rb.velocity;

        if (rb.velocity.x < minSpeed) rb.velocity = new Vector3(minSpeed, rb.velocity.y, rb.velocity.z);

        // decrease z velocity

        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z * 0.99f);


        Quaternion targetRotation = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed));

        // AddSidewaysForce();
    }

    public void ObstacleCollision(Obstacle obstacle)
    {
        // Debug.Log("Human obstacle collision with " + obstacle.name + ": stunTime = " + obstacle.stunTime + "; force = " + obstacle.CurrentPushBackForce * rb.mass);

        Vector3 dir = (transform.position - obstacle.transform.position).normalized;

        dir.y = 0;

        bumpPointLossVFX.Play();

        Stun(obstacle.stunTime);

        rb.AddForce(dir * obstacle.CurrentPushBackForce * rb.mass, ForceMode.Impulse);

        BumpedCount++;

        OnHitObstacle?.Invoke(obstacle);
    }

    void Stun(float stunTime)
    {
        this.stunTime = stunTime;

        stunVFX.gameObject.SetActive(true);
        stunVFX.Play();
    }
}