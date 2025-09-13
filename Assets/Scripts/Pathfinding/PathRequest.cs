using UnityEngine;
using System;

class PathRequest : IPathRequest
{
    private Vector3 pathStart;
    private Vector3 pathEnd;

    private Graph.Name graph;

    private Action<ABPathResult> callBack;

    public PathRequest(Vector3 pathStart, Vector3 pathEnd, Graph.Name graph, Action<ABPathResult> callBack)
    {
        this.pathStart = pathStart;
        this.pathEnd = pathEnd;
        this.graph = graph;

        this.callBack = callBack;
    }

    public void ProcessRequest(Pathfinder pathfinder)
    {
        callBack?.Invoke(pathfinder.FindPath(pathStart, pathEnd, graph));
    }
}