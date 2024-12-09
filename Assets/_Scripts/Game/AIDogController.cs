using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIDogController : DogController
{
    DogGroupObserver dogGroupObserver;


    [SerializeField] float dodgeDogDistance = 2f;


    [SerializeField] float randomFactor = 0.1f;

    private Vector3 smoothDirection;

    private void Start()
    {
        dogGroupObserver = FindObjectOfType<DogGroupObserver>();
    }

    private void Update()
    {
        InvokeRepeating(nameof(ScanSurroundings), 0.5f, 1);
        
        smoothDirection = Vector3.Lerp(smoothDirection, CalculateMoveDirection(), Time.deltaTime * 2); // Smooth transition
        movementInput = smoothDirection;
    }

    void ScanSurroundings()
    {
        
    }

    private Vector3 CalculateMoveDirection()
    {
        Vector3 targetDirection = Vector3.zero;

        // Objectives
        targetDirection += MoveTowardsClosestObjective(); // multipliers for each!
        targetDirection += MoveAwayFromOtherDogsObjectives();
        // Obstacles
        targetDirection += MoveAwayFromClosestObstacle();
        // Boid behaviour
        targetDirection += DirectionToCenterOfGroup();
        targetDirection += MoveDirectionOfGroup();
        targetDirection += MoveAwayFromClosestDogs();

        targetDirection += RandomFactor() * randomFactor;

        return targetDirection.normalized;
    }

    private Vector3 RandomFactor()
    {
        return new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
    }

    private Vector3 MoveTowardsClosestObjective()
    {
        return Vector3.zero;
    }

    private Vector3 MoveAwayFromOtherDogsObjectives()
    {
        return Vector3.zero;
    }

    private Vector3 MoveAwayFromClosestObstacle()
    {
        return Vector3.zero;
    }

    private Vector3 DirectionToCenterOfGroup()
    {
        return (dogGroupObserver.AvgDogPosition - transform.position).normalized;
    }

    private Vector3 MoveDirectionOfGroup()
    {
        return (dogGroupObserver.AvgDogDirection - transform.forward).normalized;
    }

    private Vector3 MoveAwayFromClosestDogs()
    {
        Vector3 combinedDodgeDirection = Vector3.zero;

        foreach (Transform dog in dogGroupObserver.allDogs)
        {
            if (dog == transform) continue;

            float distance = Vector3.Distance(transform.position, dog.position);

            if (distance < dodgeDogDistance)
            {
                combinedDodgeDirection += (dog.position - transform.position).normalized / distance;
            }
        }

        return combinedDodgeDirection;
    }
}
