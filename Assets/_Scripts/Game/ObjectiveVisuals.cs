using UnityEngine;

public class ObjectiveVisuals : MonoBehaviour
{
    Objective objective;


    [SerializeField] VisualByType[] objectiveVisuals;

    private void Awake()
    {
        objective = GetComponent<Objective>();
        objective.OnTypeChanged += UpdateVisuals;
    }

    private void UpdateVisuals()
    {
        foreach (VisualByType dw in objectiveVisuals)
        {
            if (dw.type == objective.ObjectiveType) dw.obj.SetActive(true);
            else dw.obj.SetActive(false);
        }
    }
}