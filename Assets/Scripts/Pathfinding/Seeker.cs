using System.Collections.Generic;
using UnityEngine;
using System;

public class Seeker : MonoBehaviour
{
    [SerializeField] private Graph.Name graph;

    public void FindPath(Vector3 target, Action<ABPathResult> callback)
    {
        Pathfinder.RequestPath(transform.position, target, graph, callback);
    }

    public void FindPaths(List<Vector3> targets, Action<List<ABPathResult>> callBack)
    {
        Pathfinder.RequestMultiPath(transform.position, targets, graph, callBack);
    }

    public Node WorldPositionToNode(Vector3 position)
    {
        return Pathfinder.WorldPositionToNode(position, graph);
    }
}