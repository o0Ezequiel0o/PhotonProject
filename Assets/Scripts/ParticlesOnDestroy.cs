using UnityEngine;
using Photon.Pun;

public class ParticlesOnDestroy : MonoBehaviour
{
    [SerializeField] private string particles;

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

        PhotonNetwork.Instantiate(particles, transform.position, Quaternion.identity);
        
        
    }
}