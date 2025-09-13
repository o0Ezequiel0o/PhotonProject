using System.Collections.Generic;
using UnityEngine;
using System;

public class Pathfinder : MonoBehaviour
{
    public static Pathfinder Instance { private set; get; }

    [Header("Config")]
    [SerializeField] private GraphInspectorConfig[] configureGraphs;
    [Header("Performance")]
    [SerializeField] private int maxPathsEachFrame;

    private Dictionary<Graph.Name, Graph> graphs = new Dictionary<Graph.Name, Graph>();

    private HashSet<Node> nodesProcessed;
    private Heap<Node> nodesToProcess;

    private Queue<IPathRequest> pathRequests = new Queue<IPathRequest>();
    private IPathRequest currentPathRequest;

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Graph.Name graph, Action<ABPathResult> callBack)
    {
        Instance.pathRequests.Enqueue(new PathRequest(pathStart, pathEnd, graph, callBack));
    }

    public static void RequestMultiPath(Vector3 pathStart, List<Vector3> pathEnds, Graph.Name graph, Action<List<ABPathResult>> callBack)
    {
        Instance.pathRequests.Enqueue(new MultiPathRequest(pathStart, pathEnds, graph, callBack));
    }

    public static Node WorldPositionToNode(Vector3 position, Graph.Name graph)
    {
        return Instance.graphs[graph].WorldPositionToNode(position);
    }

    public ABPathResult FindPath(Vector3 startPosition, Vector3 targetPosition, Graph.Name graphName)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Graph graph = graphs[graphName];

        Node startNode = graph.WorldPositionToNode(startPosition);
        Node targetNode = graph.WorldPositionToNode(targetPosition);

        nodesProcessed.Clear();
        nodesToProcess.Clear();

        if (startNode != null && targetNode != null)
        {
            nodesToProcess.Add(startNode);
        }

        while (nodesToProcess.Count > 0)
        {
            Node currentNode = nodesToProcess.RemoveFirst();
            nodesProcessed.Add(currentNode);

            if (currentNode == targetNode)
            {
                pathSuccess = true;
                break;
            }

            foreach (Node neighbour in graph.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || nodesProcessed.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                if (newMovementCostToNeighbour < neighbour.gCost || !nodesToProcess.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!nodesToProcess.Contains(neighbour))
                    {
                        nodesToProcess.Add(neighbour);
                    }
                    else
                    {
                        nodesToProcess.UpdateItem(neighbour);
                    }
                }
            }
        }

        if (pathSuccess)
        {
            waypoints = GetWaypoints(startNode, targetNode);
        }

        return new ABPathResult(waypoints, pathSuccess);
    }

    void Reset()
    {
        maxPathsEachFrame = 50;
    }

    void Awake()
    {
        CreateInstance();
        GenerateGraphs();
    }

    void Start()
    {
        int heapSize = GetBiggestGraphSize();

        nodesToProcess = new Heap<Node>(heapSize);
        nodesProcessed = new HashSet<Node>();
    }

    void Update()
    {
        if (pathRequests.Count > 0)
        {
            ProcessPathRequests();
        }
    }

    void ProcessPathRequests()
    {
        int updateAmount = Mathf.Min(maxPathsEachFrame, pathRequests.Count);

        for (int i = 0; i < updateAmount; i++)
        {
            TryProcessNextPath();
        }
    }

    void TryProcessNextPath()
    {
        currentPathRequest = pathRequests.Dequeue();
        currentPathRequest.ProcessRequest(Instance);
    }

    void CreateInstance()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void GenerateGraphs()
    {
        for (int i = 0; i < Enum.GetNames(typeof(Graph.Name)).Length; i++)
        {
            Graph newGraph;

            if (i < configureGraphs.Length)
            {
                newGraph = configureGraphs[i].CreateGraph(transform.position);
            }
            else
            {
                newGraph = Graph.GetEmptyGraph();
            }

            graphs.Add((Graph.Name)i, newGraph);
        }
    }

    int GetBiggestGraphSize()
    {
        int biggestGraphSize = 0;

        foreach (Graph graph in graphs.Values)
        {
            if (graph.MaxSize > biggestGraphSize)
            {
                biggestGraphSize = graph.MaxSize;
            }
        }

        return biggestGraphSize;
    }

    Vector3[] GetWaypoints(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        GetPath(path, startNode, endNode);

        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);

        return waypoints;
    }

    void GetPath(List<Node> path, Node startNode, Node endNode)
    {
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Add(startNode);
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();

        Vector2 oldDirection = Vector2.zero;
        Vector2 newDirection = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            newDirection.x = path[i - 1].gridPosition.x - path[i].gridPosition.x;
            newDirection.y = path[i - 1].gridPosition.y - path[i].gridPosition.y;

            if (newDirection != oldDirection)
            {
                waypoints.Add(path[i - 1].position);
            }

            oldDirection = newDirection;
        }

        return waypoints.ToArray();
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x);
        int distanceY = Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y);

        int shortestAxisDistance = Mathf.Min(distanceX, distanceY);
        int longestAxisDistance = Mathf.Max(distanceX, distanceY);

        return 14 * shortestAxisDistance + 10 * (longestAxisDistance - shortestAxisDistance);
    }

    void OnDrawGizmos()
    {
        int currentIndex = 0;

        foreach (Graph graph in graphs.Values)
        {
            if (currentIndex < configureGraphs.Length && configureGraphs[currentIndex].drawGizmos)
            {
                graph.DrawGizmos();
            }

            currentIndex += 1;
        }
    }
}

public class ABPathResult
{
    public Vector3[] waypoints;
    public bool success;

    public ABPathResult(Vector3[] waypoints, bool success)
    {
        this.waypoints = waypoints;
        this.success = success;
    }
}