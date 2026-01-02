using System.Collections.Generic;
using UnityEngine;

public class DuckMovement : MonoBehaviour
{
    public Transform pathParent; // Parent object containing waypoints
    public float moveSpeed = 3f;
    public float rotationSpeed = 2f;

    private List<Transform> waypoints;
    private int currentWaypointIndex = 0;

    void Start()
    {
        // Get all child transforms as waypoints
        waypoints = new List<Transform>();
        foreach (Transform child in pathParent)
        {
            waypoints.Add(child);
        }

        if (waypoints.Count > 0)
        {
            transform.position = waypoints[0].position; // Start at first waypoint
        }
    }

    void Update()
    {
        if (waypoints.Count == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];

        // Move towards the target waypoint
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);

        // Smoothly rotate towards the waypoint
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Check if reached waypoint
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.2f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        }
    }
}
