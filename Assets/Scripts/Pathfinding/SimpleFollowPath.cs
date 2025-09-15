using UnityEngine;
using System;

public class SimpleFollowPath : MonoBehaviour, IPathFollower
{
    [Header("Config")]
    [SerializeField] private Seeker seeker;
    [SerializeField] private EntityMove entityMove;
    [Space]
    [SerializeField] private float waypointReachDistance;
    [SerializeField] private float pathRecalculateInterval;

    private Vector3 destination;
    public Vector3[] currentPath;
    private int currentPathTargetIndex = 0;

    public bool PathCompleted => pathCompleted;
    public bool PathCanceled => pathCanceled;

    private bool pathCompleted = true;
    private bool pathCanceled = false;
    
    private Action pathCompletedCallback;
    private Action<bool> pathCalculatedCallback;

    private float pathRecalculateTimer = 0f;

    public void OnPathCompleted(Action callBack)
    {
        pathCompletedCallback = callBack;
    }

    public void StartPath(Vector3 target, Action<bool> callBack)
    {
        pathCalculatedCallback = callBack;
        seeker.FindPath(target, SearchPathResult);
    }

    public void StartPath(Vector3 target)
    {
        seeker.FindPath(target, OnPathCalculated);
    }

    public void SetPath(Vector3[] newPath)
    {
        currentPath = newPath;
        pathCompleted = false;
        pathCanceled = false;

        currentPathTargetIndex = 0;
        pathRecalculateTimer = 0f;
        
        destination = newPath[^1];
    }

    public void CancelCurrentPath()
    {
        pathCanceled = true;
        currentPathTargetIndex = 0;
        currentPath = new Vector3[0];
        entityMove.DesiredMoveDirection = Vector2.zero;
    }

    void Reset()
    {
        seeker = GetComponentInChildren<Seeker>();
        entityMove = GetComponentInChildren<EntityMove>();
    }

    void Awake()
    {
        if (seeker == null || entityMove == null)
        {
            enabled = false;
        }
    }

    void Update()
    {
        if (!LocalInstance.isHost) return;

        if (pathCanceled)
        {
            return;
        }
        
        if (!pathCompleted)
        {
            UpdatePathRecalculationInterval();
        }
        if (!pathCompleted)
        {
            UpdatePathState();
        }
        if (!pathCompleted)
        {
            MoveTowardsPathTarget();
        }
    }

    void SearchPathResult(ABPathResult path)
    {
        OnPathCalculated(path);
        pathCalculatedCallback?.Invoke(path.success);
    }

    void OnPathCalculated(ABPathResult path)
    {
        if (path.success && !pathCanceled && path.waypoints.Length > 0)
        {
            SetPath(path.waypoints);
        }
    }

    void MoveTowardsPathTarget()
    {
        entityMove.DesiredMoveDirection = currentPath[currentPathTargetIndex] - transform.position;
    }

    void UpdatePathRecalculationInterval()
    {
        pathRecalculateTimer += Time.deltaTime;

        if (pathRecalculateTimer >= pathRecalculateInterval)
        {
            StartPath(destination);
            pathRecalculateTimer = 0f;
        }
    }

    void UpdatePathState()
    {
        if (Vector3.Distance(transform.position, currentPath[currentPathTargetIndex]) <= waypointReachDistance)
        {
            currentPathTargetIndex += 1;
        }

        if (currentPathTargetIndex >= currentPath.Length)
        {
            entityMove.DesiredMoveDirection = Vector2.zero;

            currentPathTargetIndex = 0;
            pathCompleted = true;
            
            pathCompletedCallback?.Invoke();
        }
    }

    void OnDrawGizmos()
    {
        if (currentPath == null || pathCompleted)
        {
            return;
        }

        for (int i = currentPathTargetIndex; i < currentPath.Length; i++)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(currentPath[i], 0.2f);

            if (i == currentPathTargetIndex)
            {
                Gizmos.DrawLine(transform.position, currentPath[i]);
            }
            else
            {
                Gizmos.DrawLine(currentPath[i-1], currentPath[i]);
            }
        }
    }
}