using UnityEngine;

public class ParticlesOnDestroy : MonoBehaviour
{
    [SerializeField] private GameObject particles;

    private void Awake()
    {
        if (TryGetComponent(out Health health))
        {
            health.onDeath += CreateDestroyParticles;
        }
    }

    private void CreateDestroyParticles()
    {
        if (particles == null) return;

        Instantiate(particles, transform.position, Quaternion.identity);
    }
}