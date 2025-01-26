using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class HumanMovement : MonoBehaviour
{
    public Transform LeashAttachmentPoint;


    [SerializeField] float startWalkingDelay = 3f;
    [SerializeField] float minSpeed = 1f;
    public float speed = 5f; // Speed of movement
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
        Debug.Log("stop");

        MoveForward();
    }

    private void MoveForward()
    {
        if (PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient) return;

        if (isStunned)
        {
            return;
        }

        Vector3 direction = Vector3.right;
        
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * direction);

        if (rb.velocity.x < minSpeed) rb.velocity = new Vector3(minSpeed, rb.velocity.y, rb.velocity.z);

        // decrease z velocity

        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z * 0.99f);


        Quaternion targetRotation = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * speed));
    }

    public void ObstacleCollision(Obstacle obstacle)
    {
        Debug.Log("Human obstacle collision with " + obstacle.name + ": stunTime = " + obstacle.stunTime + "; force = " + obstacle.CurrentPushBackForce * rb.mass);

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