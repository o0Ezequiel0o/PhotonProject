using UnityEngine;

public abstract class EntityMove : MonoBehaviour
{
    [Header("Dependency")]
    [SerializeField] protected Physics physics;

    [Header("Settings")]
    [SerializeField] public Stat moveSpeed;

    public abstract Vector2 MoveDirection { get; }
    public Vector2 Velocity => physics.Velocity;

    public Vector2 LastDesiredMoveDirection
    {
        get;
        private set;
    }

    private Vector2 desiredMoveDirection;
    public Vector2 DesiredMoveDirection
    {
        get
        {
            return desiredMoveDirection;
        }
        set
        {
            desiredMoveDirection = value;

            if (value != Vector2.zero)
            {
                LastDesiredMoveDirection = value;
            }
        }
    }

    protected virtual void Awake() { }

    protected virtual void Reset()
    {
        physics = GetComponentInChildren<Physics>();
    }
}