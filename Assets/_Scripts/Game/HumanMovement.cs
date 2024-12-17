using System;
using System.Collections.Generic;
using UnityEngine;

public class HumanMovement : MonoBehaviour
{
    public float speed = 5f; // Speed of movement
    private Rigidbody rb; // Rigidbody for physical movement
    private Queue<Transform> waypoints = new Queue<Transform>(); // Queue of waypoints

    private Transform currentTarget; // Current target waypoint

    float stunTime;
    bool isStunned => stunTime > 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody component found on this GameObject. Please attach one.");
        }
    }

    void FixedUpdate()
    {
        stunTime -= Time.fixedDeltaTime;

        if (isStunned) return;

        if (currentTarget != null)
        {
            MoveTowardsCurrentTarget();
        }
        else if (waypoints.Count > 0)
        {
            SetNextWaypoint();
        }
    }

    private void MoveTowardsCurrentTarget()
    {
        if (isStunned)
        {
            return;
        }

        Vector3 direction = (currentTarget.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);

                // Optional: Rotate towards the target
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * speed));
    }

    private void SetNextWaypoint()
    {
        if (waypoints.Count > 0)
        {
            currentTarget = waypoints.Dequeue();
        }
    }

    public void AddWaypoints(List<Transform> newWaypoints)
    {
        foreach (var waypoint in newWaypoints)
        {
            waypoints.Enqueue(waypoint);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger enter: " + other.name);

        if (currentTarget != null && other.transform == currentTarget)
        {
            currentTarget = null;
        }
    }

    internal void Stun(float stunTime)
    {
        this.stunTime = stunTime;
    }
}