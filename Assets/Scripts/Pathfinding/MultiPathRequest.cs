using System.Collections.Generic;
using UnityEngine;
using System;

class MultiPathRequest : IPathRequest
{
    private Vector3 pathStart;
    private List<Vector3> pathEnds;

    private Graph.Name graph;

    private Action<List<ABPathResult>> callBack;

    public MultiPathRequest(Vector3 pathStart, List<Vector3> pathEnds, Graph.Name graph, Action<List<ABPathResult>> callBack)
    {
        this.pathStart = pathStart;
        this.pathEnds = pathEnds;
        this.graph = graph;
        
        this.callBack = callBack;
    }

    public void ProcessRequest(Pathfinder pathfinder)
    {
        List<ABPathResult> paths = new List<ABPathResult>();

        for (int i = 0; i < pathEnds.Count; i++)
        {
            paths.Add(pathfinder.FindPath(pathStart, pathEnds[i], graph));
        }

        callBack?.Invoke(paths);
    }
}