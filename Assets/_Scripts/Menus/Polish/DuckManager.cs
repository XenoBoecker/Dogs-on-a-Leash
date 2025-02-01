using System.Collections.Generic;
using UnityEngine;

public class DuckManager : MonoBehaviour
{
    public List<DucklingMovement> ducklings;
    public Transform mainDuck;

    void Start()
    {
        foreach (DucklingMovement duckling in ducklings)
        {
            duckling.targetDuck = mainDuck;
            duckling.allDucklings = ducklings;
        }
    }
}