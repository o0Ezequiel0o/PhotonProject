using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(IPathFollower))]
public class TargetFollower : TargetFollowerBase
{
    [SerializeField] private EntityMove entityMove;

    [Header("Line of Sight")]
    [SerializeField] private bool UseLineOfSight;
    [SerializeField] private LayerMask blockLayers;

    bool targetChangedNode = false;

    protected override void Reset()
    {
        base.Reset();
        entityMove = GetComponentInChildren<EntityMove>();
    }

    void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        UpdateTargetPosition();

        if (!targetChangedNode)
        {
            return;
        }

        if (UseLineOfSight && HasLineOfSight())
        {
            pathFollower.CancelCurrentPath();
            entityMove.DesiredMoveDirection = (target.position - transform.position).normalized;
        }
        else
        {
            pathFollower.StartPath(target.position);

            lastTargetPosition = target.position;
            lastTargetNode = currentTargetNode;
        }
    }

    void UpdateTargetPosition()
    {
        targetChangedNode = false;

        if (target == null || target.position == lastTargetPosition)
        {
            return;
        }

        currentTargetNode = seeker.WorldPositionToNode(target.position);

        if (currentTargetNode == lastTargetNode)
        {
            return;
        }

        targetChangedNode = true;
    }

    bool HasLineOfSight()
    {
        return !Physics2D.Linecast(transform.position, target.position, blockLayers);
    }
}