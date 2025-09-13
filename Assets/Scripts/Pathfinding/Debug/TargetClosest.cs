using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IPathFollower))]
public class TargetClosest : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private Seeker seeker;
    [Header("Targeting")]
    [SerializeField] private List<Transform> targets;
    [SerializeField] private CalculationMode distanceCalculationMode;

    private IPathFollower pathFollower;

    void Reset()
    {
        seeker = GetComponentInChildren<Seeker>();
    }

    void Awake()
    {
        pathFollower = GetComponentInChildren<IPathFollower>();
    }

    void Start()
    {
        FindShortestPath();
    }

    List<Vector3> GetTargetsPosition()
    {
        List<Vector3> targetsVectors = new List<Vector3>();

        for (int i = targets.Count - 1; i >= 0; i--)
        {
            if (targets[i] == null)
            {
                targets.RemoveAt(i);
                continue;
            }

            targetsVectors.Add(targets[i].position);
        }

        return targetsVectors;
    }

    void FindShortestPath()
    {
        List<Vector3> targetsVectors = GetTargetsPosition();

        if (targetsVectors.Count == 0)
        {
            return;
        }

        switch (distanceCalculationMode)
        {
            case CalculationMode.Distance:
                GetClosestTargetByDistance(targetsVectors);
                break;

            case CalculationMode.PathDistance:
                seeker.FindPaths(targetsVectors, GetClosestTargetByPathDistance);
                break;
        }
    }

    void GetClosestTargetByDistance(List<Vector3> targetsPositions)
    {
        Vector3 closestTargetPosition = targetsPositions[0];
        float shortestTargetDistance = float.MaxValue;

        for (int i = 0; i < targetsPositions.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, targetsPositions[i]);

            if (distance < shortestTargetDistance)
            {
                closestTargetPosition = targetsPositions[i];
                shortestTargetDistance = distance;
            }
        }

        seeker.FindPath(closestTargetPosition, OnShortestPathFound);
    }

    void GetClosestTargetByPathDistance(List<ABPathResult> pathResults)
    {
        ABPathResult shortestPath = pathResults[0];
        float shortestPathDistance = float.MaxValue;

        for (int i = 0; i < pathResults.Count; i++)
        {
            float distance = 0f;

            for (int j = 0; j < pathResults[i].waypoints.Length; j++)
            {
                if (j == 0)
                {
                    distance += Vector3.Distance(transform.position, pathResults[i].waypoints[j]);
                }
                else
                {
                    distance += Vector3.Distance(pathResults[i].waypoints[j - 1], pathResults[i].waypoints[j]);
                }
            }

            if (distance < shortestPathDistance)
            {
                shortestPath = pathResults[i];
                shortestPathDistance = distance;
            }
        }

        OnShortestPathFound(shortestPath);
    }

    void OnShortestPathFound(ABPathResult shortestPath)
    {
        pathFollower.SetPath(shortestPath.waypoints);
    }

    private enum CalculationMode
    {
        Distance,
        PathDistance,
    }
}