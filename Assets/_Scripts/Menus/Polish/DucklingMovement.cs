using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DucklingMovement : MonoBehaviour
{
    public Transform targetDuck; // The main duck to follow
    public List<DucklingMovement> allDucklings; // Other ducklings for flock behavior

    public float followDistance = 2f;
    public float separationDistance = 1f;
    public float moveSpeed = 2.5f;
    public float rotationSpeed = 3f;
    public float wanderRange = 1.5f;
    public float wanderChance = 0.2f; // 20% chance per second


    [SerializeField] float wantedDistToMamaDuck = 1f;

    [SerializeField] float speedBostStartDist = 3f;
    [SerializeField] float speedBoostMultiplier;

    [SerializeField] float speedUpTime = 0.3f;

    [SerializeField] float speedDownTime = 0.3f;
    [SerializeField] float distRandomizerMultiplier = 0.3f;
    float distRandor = 1;

    private Vector3 wanderTarget;
    private bool isBoosting = false;

    void Update()
    {
        if ((Vector3.Distance(transform.position, targetDuck.position) > speedBostStartDist * distRandor) && !isBoosting)
        {
            StartCoroutine(ShortSpeedBoost());
        }

        ApplyBoidBehavior();
    }

    void ApplyBoidBehavior()
    {
        Vector3 followForce = (targetDuck.position - transform.position).normalized * followDistance;
        Vector3 separationForce = Vector3.zero;
        int nearbyDucklings = 0;

        foreach (var duckling in allDucklings)
        {
            if (duckling == this) continue;

            float distance = Vector3.Distance(transform.position, duckling.transform.position);
            if (distance < separationDistance)
            {
                separationForce += (transform.position - duckling.transform.position).normalized / distance;
                nearbyDucklings++;
            }
        }

        if (nearbyDucklings > 0)
        {
            separationForce /= nearbyDucklings;
        }

        Vector3 finalDirection = followForce + separationForce;

        finalDirection = new Vector3(finalDirection.x, 0, finalDirection.z);

        finalDirection = finalDirection.normalized;

        // Move the duckling
        transform.position = Vector3.MoveTowards(transform.position, transform.position + finalDirection, moveSpeed * Time.deltaTime);

        // Smoothly rotate towards movement direction
        if (finalDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(finalDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    IEnumerator ShortSpeedBoost()
    {
        if (isBoosting) yield break;
        isBoosting = true;

        distRandor = 1 + Random.Range(-distRandomizerMultiplier, +distRandomizerMultiplier);

        Debug.Log("Start Boost");

        float distToMamaDuck = Mathf.Infinity;

        moveSpeed *= speedBoostMultiplier;

        while (distToMamaDuck > wantedDistToMamaDuck)
        {
            distToMamaDuck = Vector3.Distance(transform.position, targetDuck.position);
            
            yield return null;
        }

        Debug.Log("End Boost");

        float speed = moveSpeed;
        float goalSpeed = moveSpeed / speedBoostMultiplier;

        for (float i = 0; i < speedDownTime; i+=Time.deltaTime)
        {
            moveSpeed = Mathf.Lerp(speed, goalSpeed, i / speedDownTime);
            yield return null;
        }

        isBoosting = false;
        moveSpeed = goalSpeed;
    }
    /*
     * 
    IEnumerator ShortSpeedBoost()
    {
        if (isBoosting) yield break;
        isBoosting = true;

        distRandor = 1 + Random.Range(-distRandomizerMultiplier, +distRandomizerMultiplier);


        float distToMamaDuck = Mathf.Infinity;

        float goalSpeed = baseMoveSpeed * speedBoostMultiplier;
        Debug.Log("Start Boost: " + goalSpeed);

        for (float i = 0; i < speedUpTime; i+= Time.deltaTime)
        {
            currentMoveSpeed = Mathf.Lerp(baseMoveSpeed, goalSpeed, i / speedUpTime);
            yield return null;
        }

        currentMoveSpeed = baseMoveSpeed * speedBoostMultiplier;

        while (distToMamaDuck > wantedDistToMamaDuck)
        {
            distToMamaDuck = Vector3.Distance(transform.position, targetDuck.position);
            
            yield return null;
        }

        for (float i = 0; i < speedDownTime; i += Time.deltaTime)
        {
            currentMoveSpeed = Mathf.Lerp(goalSpeed, baseMoveSpeed, i / speedUpTime);
            yield return null;
        }

        currentMoveSpeed = baseMoveSpeed;

        Debug.Log("End Boost: " + currentMoveSpeed);

        isBoosting = false;
    }
    */

    IEnumerator WanderRoutine()
    {
        isBoosting = true;

        wanderTarget = transform.position + new Vector3(Random.Range(-wanderRange, wanderRange), 0, Random.Range(-wanderRange, wanderRange));

        float wanderTime = Random.Range(1f, 3f);
        float timer = 0f;

        while (timer < wanderTime)
        {
            transform.position = Vector3.MoveTowards(transform.position, wanderTarget, moveSpeed * Time.deltaTime);

            Vector3 direction = (wanderTarget - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            if (Vector3.Distance(transform.position, wanderTarget) < 0.2f)
            {
                break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        isBoosting = false;
    }
}
