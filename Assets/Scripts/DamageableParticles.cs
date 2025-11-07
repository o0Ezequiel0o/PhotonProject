using UnityEngine;

public class DamageableParticles : MonoBehaviour
{
    [SerializeField] private GameObject particles;

    private void Awake()
    {
        if (TryGetComponent(out Health health))
        {
            health.onDamageTaken += SpawnParticles;
        }
    }

    private void SpawnParticles()
    {
        Instantiate(particles, transform.position, Quaternion.identity);
    }
}