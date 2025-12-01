using Photon.Pun;
using UnityEngine;

public class EntityMoveRotation : EntityMove
{
    [SerializeField] private float baseRotationSpeed;
    [SerializeField] public bool slowWhenNotFacing;

    public override Vector2 MoveDirection => moveDirection;
    private Vector2 moveDirection = Vector2.zero;

    private StatMultiplier speedMultiplier = new StatMultiplier();

    protected override void Awake()
    {
        base.Awake();
        moveSpeed.AddMultiplier(speedMultiplier);
    }

    void FixedUpdate()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            UpdateMovement();
        }
    }

    void UpdateMovement()
    {
        float expectedRotation = Mathf.Atan2(DesiredMoveDirection.y, DesiredMoveDirection.x) * Mathf.Rad2Deg - 90f;
        float rotationOffset = Mathf.Abs(Mathf.DeltaAngle(expectedRotation, physics.Rotation));

        if (slowWhenNotFacing)
        {
            float newSpeedMultiplier = 1 - (Mathf.Min(rotationOffset, 180f) / 180f);
            speedMultiplier.UpdateMultiplier(newSpeedMultiplier);
        }

        moveDirection = transform.up.normalized;

        physics.Rotate(expectedRotation, baseRotationSpeed * Time.fixedDeltaTime);
        physics.SetConstantSpeed(moveSpeed.Value * moveDirection);
    }
}