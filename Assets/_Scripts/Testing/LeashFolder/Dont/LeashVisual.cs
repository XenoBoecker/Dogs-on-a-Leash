using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeashVisual : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform startPoint;
    public Transform endPoint;
    public LeashManager leashManager;
    public int leashVisualSegment = 10;

    public Material[] leashMaterials;

    private List<Vector3> leashPoints;
    private List<Vector3> currentPositions;
    private float leashLength;
    private float maxLeashLength = 10;
    public float moveSpeed = 5f; // Speed at which points move towards the desired position

    private List<Vector3> finalPoints;

    // Start is called before the first frame update
    void Start()
    {
        endPoint = FindObjectOfType<HumanMovement>().LeashAttachmentPoint;
        startPoint = GetComponent<PlayerDogVisuals>().LeashAttachmentPoint;
        leashManager = gameObject.GetComponent<LeashManager>();
        maxLeashLength = leashManager.GetMaxLeashLength();
        leashPoints = new List<Vector3>();
        currentPositions = new List<Vector3>(new Vector3[leashVisualSegment]);
        lineRenderer.positionCount = leashVisualSegment;
        lineRenderer.material = leashMaterials[gameObject.GetComponent<PlayerDogVisuals>().GetColorID()];

        // Set the width of the line renderer
        lineRenderer.startWidth = 0.2f; // Adjust this value as needed
        lineRenderer.endWidth = 0.2f;   // Adjust this value as needed
    }

    // Update is called once per frame
    void Update()
    {
        leashLength = leashManager.GetCurrentLength();
        maxLeashLength = leashManager.GetMaxLeashLength();
        UpdateLeashPoints();
        ApplyCurve();

        gameObject.GetComponent<RopeRenderer>().RenderRope(finalPoints.ToArray(), 0.07f);
    }

    void UpdateLeashPoints()
    {
        leashPoints.Clear();
        leashPoints.Add(startPoint.position);

        foreach (GameObject point in leashManager.leashSegments)
        {
            if (point != null)
            {
                Vector3 pointPosition = point.transform.position;
                pointPosition.y = 0; // Ignore the y-coordinate of the leash segment
                leashPoints.Add(pointPosition);
            }
        }
        leashPoints.Add(endPoint.position);

        List<Vector3> positions = new List<Vector3>();

        for (int i = 0; i < leashVisualSegment; i++)
        {
            float t = (float)i / (leashVisualSegment - 1);
            Vector3 position = GetPositionAlongLeash(t);
            positions.Add(position);
        }

        for (int i = 0; i < positions.Count; i++)
        {
            currentPositions[i] = Vector3.Lerp(currentPositions[i], positions[i], Time.deltaTime * moveSpeed);
        }

        lineRenderer.positionCount = currentPositions.Count;
        lineRenderer.SetPositions(currentPositions.ToArray());
    }

    Vector3 GetPositionAlongLeash(float t)
    {
        float totalLength = 0;
        for (int i = 0; i < leashPoints.Count - 1; i++)
        {
            totalLength += Vector3.Distance(leashPoints[i], leashPoints[i + 1]);
        }

        float targetLength = t * totalLength;
        float currentLength = 0;

        for (int i = 0; i < leashPoints.Count - 1; i++)
        {
            float segmentLength = Vector3.Distance(leashPoints[i], leashPoints[i + 1]);
            if (currentLength + segmentLength >= targetLength)
            {
                float segmentT = (targetLength - currentLength) / segmentLength;
                return Vector3.Lerp(leashPoints[i], leashPoints[i + 1], segmentT);
            }
            currentLength += segmentLength;
        }

        return leashPoints[leashPoints.Count - 1];
    }

    void ApplyCurve()
    {
        Vector3[] positions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(positions);

        float stretchFactor = Mathf.Clamp01(leashLength / maxLeashLength);

        for (int i = 0; i < positions.Length - 1; i++)
        {
            float t = (float)i / (positions.Length - 1);
            float height = Mathf.Clamp(Mathf.Sin(t * Mathf.PI) * (1 - stretchFactor) * maxLeashLength / 2, 0, maxLeashLength / 2);
            positions[i].y = Mathf.Max(positions[i].y - height, 0.2f);
        }

        finalPoints = new List<Vector3>(positions);

        lineRenderer.SetPositions(positions);
    }

    // void MovePointsTowardsDesiredPositions()
    // {
    //     for (int i = 0; i < currentPositions.Count; i++)
    //     {
    //         if (i < leashPoints.Count)
    //         {
    //             currentPositions[i] = Vector3.Lerp(currentPositions[i], leashPoints[i], Time.deltaTime * moveSpeed);
    //         }
    //     }
    //     lineRenderer.SetPositions(currentPositions.ToArray());
    // }
}