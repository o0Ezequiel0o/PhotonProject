using System.Collections.Generic;
using UnityEngine;

public abstract class TargetFollowerBase : MonoBehaviour, ITargetFollower
{
    [Header("Config")]
    [SerializeField] protected Seeker seeker;

    protected IPathFollower pathFollower;

    protected Transform target;
    protected Vector3 lastTargetPosition;

    protected Node currentTargetNode;
    protected Node lastTargetNode;

    public virtual void SetTarget(Transform newTarget)
    {
        target = newTarget;
        currentTargetNode = null;

        lastTargetPosition = target.position;
        lastTargetNode = seeker.WorldPositionToNode(target.position);

        pathFollower.StartPath(target.position);
    }

    public virtual void RemoveTarget()
    {
        target = null;
    }

    protected Transform GetClosestTargetByDistance(List<Transform> targets)
    {
        Transform closestTarget = targets[0];
        float closestTargetDistance = float.MaxValue;

        for (int i = 0; i < targets.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, targets[i].position);

            if (distance < closestTargetDistance)
            {
                closestTarget = targets[i];
                closestTargetDistance = distance;
            }
        }

        return closestTarget;
    }

    protected virtual void Reset()
    {
        seeker = GetComponentInChildren<Seeker>();
    }

    protected virtual void Awake()
    {
        pathFollower = GetComponentInChildren<IPathFollower>();

        if (seeker == null || pathFollower == null)
        {
            enabled = false;
        }
    }
}