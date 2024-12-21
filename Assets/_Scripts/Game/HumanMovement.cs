using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HumanMovement : MonoBehaviour
{

    [SerializeField] float minSpeed = 1f;
    public float speed = 5f; // Speed of movement
    private Rigidbody rb; // Rigidbody for physical movement
    private Queue<Transform> waypoints = new Queue<Transform>(); // Queue of waypoints

    private Transform currentTarget; // Current target waypoint

    float stunTime;
    bool isStunned => stunTime > 0;

    public event Action OnEndGame;
    public event Action<Obstacle> OnHitObstacle;

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
        
        rb.rotation = Quaternion.Euler(0, rb.rotation.eulerAngles.y, 0); // only  y rotation

        if (isStunned) return;

        if (currentTarget != null)
        {
            MoveTowardsCurrentTarget();
        }
        else
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

        //Vector3 direction = (currentTarget.position - transform.position).normalized;

        Vector3 direction = Vector3.right;
        
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);

        if (rb.velocity.x < minSpeed) rb.velocity = new Vector3(minSpeed, rb.velocity.y, rb.velocity.z);

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
        else
        {
            EndGame();
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
        if (currentTarget != null && other.transform == currentTarget)
        {
            currentTarget = null;
        }
    }

    public void ObstacleCollision(Obstacle obstacle)
    {
        Debug.Log("Human obstacle collision: stunTime = " + obstacle.stunTime + "; force = " + obstacle.CurrentPushBackForce);

        Vector3 dir = (transform.position - obstacle.transform.position).normalized;

        dir.y = 0;

        Stun(obstacle.stunTime);

        rb.AddForce(dir * obstacle.CurrentPushBackForce, ForceMode.Impulse);

        OnHitObstacle?.Invoke(obstacle);
    }

    void Stun(float stunTime)
    {
        this.stunTime = stunTime;

    }

    private void EndGame()
    {
        Debug.Log("Game OVer");

        OnEndGame?.Invoke();
    }
}