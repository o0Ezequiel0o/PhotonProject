using UnityEngine;

public class EntityMoveSnap : EntityMove
{
    public override Vector2 MoveDirection => DesiredMoveDirection;

    void FixedUpdate()
    {
        UpdateMovement();
    }

    void UpdateMovement()
    {
        physics.SetConstantSpeed(moveSpeed.Value * DesiredMoveDirection.normalized);
    }
}