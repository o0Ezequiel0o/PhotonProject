using UnityEngine;

[RequireComponent(typeof(IPathFollower))]
public class SimpleTargetFollower : TargetFollowerBase
{
    void Update()
    {
        if (target == null || target.position == lastTargetPosition)
        {
            return;
        }

        currentTargetNode = seeker.WorldPositionToNode(target.position);

        if (currentTargetNode == lastTargetNode)
        {
            return;
        }

        pathFollower.StartPath(target.position);

        lastTargetPosition = target.position;
        lastTargetNode = currentTargetNode;
    }
}