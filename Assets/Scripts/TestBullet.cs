using UnityEngine;

public class TestBullet : MonoBehaviour
{
    [SerializeField] private float speed;

    private Vector2 direction;

    public void LaunchProjectile(Vector2 direction)
    {
        this.direction = direction;
    }

    void Update()
    {
        transform.position += speed * (Vector3)direction * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.SetActive(false);
    }
}