using UnityEngine;

public abstract class EntityAim : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Stat rotationSpeed;

    public abstract Vector2 AimDirection { get; }

    private Vector2 desiredAimDirection = Vector2.up;

    public void AimTowards(Vector2 desiredDirection)
    {
        desiredAimDirection = desiredDirection;
    }

    void FixedUpdate()
    {
        float desiredRotation = (Mathf.Atan2(desiredAimDirection.y, desiredAimDirection.x) * Mathf.Rad2Deg) - 90f;
        UpdateRotationInternal(desiredRotation, rotationSpeed.Value * Time.fixedDeltaTime);
    }

    protected abstract void UpdateRotationInternal(float desiredRotation, float step);
}