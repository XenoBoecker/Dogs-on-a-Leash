using System;
using UnityEngine;

public class AIDogController : DogController
{
    private void Update()
    {
        InvokeRepeating(nameof(ScanSurroundings), 0, 1);

        movementInput = CalculateMoveDirection();
    }

    void ScanSurroundings()
    {
    
    }

    private Vector2 CalculateMoveDirection()
    {
        Vector2 targetDirection = Vector2.zero;

        // Objectives
        targetDirection += MoveTowardsClosestObjective();
        targetDirection += MoveAwayFromOtherDogsObjectives();
        // Obstacles
        targetDirection += MoveAwayFromClosestObstacle();
        // Boid behaviour
        targetDirection += DirectionToCenterOfGroup();
        targetDirection += MoveDirectionOfGroup();
        targetDirection += MoveAwayFromClosestDogs();

        return targetDirection;
    }

    private Vector2 MoveTowardsClosestObjective()
    {
        throw new NotImplementedException();
    }

    private Vector2 MoveAwayFromOtherDogsObjectives()
    {
        throw new NotImplementedException();
    }

    private Vector2 MoveAwayFromClosestObstacle()
    {
        throw new NotImplementedException();
    }

    private Vector2 DirectionToCenterOfGroup()
    {
        throw new NotImplementedException();
    }

    private Vector2 MoveDirectionOfGroup()
    {
        throw new NotImplementedException();
    }

    private Vector2 MoveAwayFromClosestDogs()
    {
        throw new NotImplementedException();
    }
}