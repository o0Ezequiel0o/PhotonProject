using UnityEngine;

public interface ITargetFollower
{
    abstract void SetTarget(Transform newTarget);

    abstract void RemoveTarget();
}