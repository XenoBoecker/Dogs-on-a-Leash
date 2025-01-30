using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class RopeRenderer : MonoBehaviour
{
    [Min(3)] [SerializeField] private int m_RopeSegmentSides;
    [SerializeField] private Material ropeMaterial;

    [SerializeField] private Gradient[] leashColorGradients;

    private MeshFilter m_MeshFilter;
    private MeshRenderer m_MeshRenderer;
    private Mesh m_RopeMesh;
    private LeashVisual leashVisual;
    private Vector3[] m_Vertices;
    private int[] m_Triangles;

    private Material myLeashMat;
    private Gradient myLeashGradient;

    LeashManager leashManager;

    PlayerDogVisuals myDogVisual;
    


    private float m_Angle;
    private int m_NodeCount;
    private bool m_IsInitialized;

    private void Awake()
    {
        m_MeshFilter = GetComponent<MeshFilter>();
        m_MeshRenderer = GetComponent<MeshRenderer>();

        m_RopeMesh = new Mesh();
        m_MeshRenderer.material = ropeMaterial;
        
        m_Angle = ((m_RopeSegmentSides - 2) * 180) / m_RopeSegmentSides;
        m_IsInitialized = false;
    }

    private void Start()
    {
        leashVisual = GetComponent<LeashVisual>();
        m_Vertices = new Vector3[leashVisual.leashVisualSegment * m_RopeSegmentSides];
        m_Triangles = new int[m_RopeSegmentSides * (leashVisual.leashVisualSegment - 1) * 6];

        myLeashMat = new Material(ropeMaterial);
        gameObject.GetComponent<RopeRenderer>().m_MeshRenderer.material = myLeashMat;

        myDogVisual = GetComponent<PlayerDogVisuals>();

        leashManager = gameObject.GetComponent<LeashManager>();

        myLeashGradient = leashColorGradients[myDogVisual.GetColorID()];

    }

    void Update()
    {
        UpdateLeashColor();
    }

    public void RenderRope(Vector3[] nodes, float radius)
    {
        if (m_Vertices is null || m_Triangles is null)
            return;

        ComputeVertices(nodes, radius);
        
        if(!m_IsInitialized){
            ComputeTriangles();
            m_IsInitialized = true;
        }
        
        SetupMeshFilter();
    }

    private void ComputeVertices(Vector3[] nodes, float radius)
    {
        var angle = (360f / m_RopeSegmentSides) * Mathf.Deg2Rad;

        for (int i = 0; i < m_Vertices.Length; i++)
        {
            var nodeindex = i / m_RopeSegmentSides;
            var sign = nodeindex == nodes.Length - 1 ? -1 : 1;
            // Debug.Log($"Node Index: {nodeindex}, Vert Index: {i} , {m_Vertices[i]}");
            
            var currNodePosition = transform.InverseTransformPoint(nodes[nodeindex]);
            var normalOfPlane =
                (sign * transform.InverseTransformPoint(nodes[nodeindex]) + -sign * transform.InverseTransformPoint(nodes[nodeindex + (nodeindex == nodes.Length - 1 ? -1 : 1)]))
                .normalized;

            var u = Vector3.Cross(normalOfPlane, Vector3.forward).normalized;
            var v = Vector3.Cross(u, normalOfPlane).normalized;

            m_Vertices[i] = currNodePosition + radius * (float)Math.Cos(angle * (i % m_RopeSegmentSides)) * u +
                            radius * (float)Math.Sin(angle * (i % m_RopeSegmentSides)) * v;
        }
    }

    private void ComputeTriangles()
    {
        var tn = 0;

        for (int i = 0; i < m_Vertices.Length - m_RopeSegmentSides; i++)
        {
            var nexti = (i + 1) % m_RopeSegmentSides == 0 ? i - m_RopeSegmentSides + 1 : i + 1;

            m_Triangles[tn] = i;
            m_Triangles[tn + 1] = nexti + m_RopeSegmentSides;
            m_Triangles[tn + 2] = i + m_RopeSegmentSides;

            m_Triangles[tn + 3] = i;
            m_Triangles[tn + 4] = nexti;
            m_Triangles[tn + 5] = nexti + m_RopeSegmentSides;

            tn += 6;
        }
    }

    private void SetupMeshFilter()
    {
        m_RopeMesh.Clear();
        m_RopeMesh.vertices = m_Vertices;
        m_RopeMesh.triangles = m_Triangles;

        m_MeshFilter.mesh = m_RopeMesh;
        m_MeshFilter.mesh.RecalculateBounds();
        m_MeshFilter.mesh.RecalculateNormals();
    }

    void UpdateLeashColor()
    {
        float currentLength = leashManager.GetCurrentLength();
        float maxLength = leashManager.GetMaxLeashLength();
        float t = Mathf.Clamp01(currentLength / maxLength);
        Color color = myLeashGradient.Evaluate(t);
        myLeashMat.color = color;
    }
}