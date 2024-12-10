using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIDogController : DogController
{
    DogGroupObserver dogGroupObserver;


    [SerializeField] float dodgeDogDistance = 2f;
    
    [SerializeField] float randomFactor = 0.1f;

    [SerializeField] float objectiveFactor = 3f;

    List<Objective> myObjectives = new List<Objective>();
    Objective closestObjective;

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
        if(myObjectives.Count == 0)
        {
            myObjectives = FindObjectsOfType<Objective>().Where(o => o.ObjectiveType == GetComponent<Dog>().DogData.objectiveType).ToList<Objective>();

            for (int i = 0; i < myObjectives.Count; i++)
            {
                myObjectives[i].OnObjectiveCollected += RemoveObjective;
            }
        }

        if (myObjectives.Count == 0)
        {
            Debug.Log("No more Objectives to be found");
            return;
        }

        closestObjective = myObjectives[0];
        float closestDistance = Vector3.Distance(transform.position, closestObjective.transform.position);

        for (int i = 1; i < myObjectives.Count; i++)
        {
            float dist = Vector3.Distance(transform.position, myObjectives[i].transform.position);

            if (dist < closestDistance)
            {
                closestObjective = myObjectives[i];

                closestDistance = dist;
            }
        }
    }

    private void RemoveObjective(Objective objective)
    {
        myObjectives.Remove(objective);
    }

    private Vector3 CalculateMoveDirection()
    {
        Vector3 targetDirection = Vector3.zero;

        // Objectives
        targetDirection += MoveTowardsClosestObjective() * objectiveFactor; // multipliers for each!
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
        if (closestObjective == null) ScanSurroundings();

        return (closestObjective.transform.position - transform.position).normalized;
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
