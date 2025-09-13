using UnityEngine;
using System;

[Serializable]
public struct GraphInspectorConfig
{
    [Header("Position")]
    public Vector2 localPosition;

    [Header("Dimentions")]
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public float entityRadius;

    [Header("Colliders")]
    public int erosionIterations;
    public LayerMask unwalkableLayer;

    [Header("Runtime-Only")]
    public bool drawGizmos;

    public Graph CreateGraph(Vector3 worldPosition)
    {
        Vector3 localPosition = (Vector2)worldPosition + this.localPosition;
        return new Graph(localPosition, unwalkableLayer, erosionIterations, gridWorldSize, nodeRadius, entityRadius);
    }
}