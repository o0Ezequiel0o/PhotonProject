using UnityEngine;

public class Physics : MonoBehaviour
{
    [Header("Dependency")]
    [SerializeField] private Rigidbody2D rigidBody;
    [Header("Settings")]
    [SerializeField] private float forceReceivedMultiplier = 1f;
    [SerializeField] private float linearDamping = 0.2f;
    [Space]
    [SerializeField] private bool useConstantSpeed = true;

    public float Rotation => rigidBody.rotation;
    public Vector2 Velocity => rigidBody.velocity;

    private Vector2 constantSpeed;
    private Vector2 forces;

    private Vector2 velocity;

    public void SetConstantSpeed(Vector2 speed)
    {
        constantSpeed = speed;
    }

    public void AddForce(Vector2 force)
    {
        forces += force * forceReceivedMultiplier / rigidBody.mass;
    }

    public void Rotate(float angle, float step)
    {
        rigidBody.rotation = Mathf.MoveTowardsAngle(rigidBody.rotation, angle, step);
    }

    void Reset()
    {
        rigidBody = GetComponentInChildren<Rigidbody2D>();
    }

    void OnValidate()
    {
        ClampValues();
    }

    void FixedUpdate()
    {
        UpdateForces();
        UpdateVelocity();
    }

    void ClampValues()
    {
        linearDamping = Mathf.Max(0, linearDamping);
    }

    void UpdateForces()
    {
        forces /= 1 + linearDamping;

        if (Mathf.Abs(forces.x) <= 0.01f && Mathf.Abs(forces.y) <= 0.01f)
        {
            forces = Vector2.zero;
        }
    }

    void UpdateVelocity()
    {
        velocity = forces;

        if (useConstantSpeed)
        {
            velocity += constantSpeed;
        }

        rigidBody.velocity = velocity;
    }
}