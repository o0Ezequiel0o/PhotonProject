using Photon.Pun;
using UnityEngine;

public class HealthBarUpdater : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private Health health;

    private PlayerHealthBar healthBar;

    private void Awake()
    {
        healthBar = FindFirstObjectByType<PlayerHealthBar>();

        if (healthBar == null) enabled = false;
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            health.onHealthChanged += UpdateHealthBar;
            health.onDamageTaken += UpdateHealthBar;
        }

        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthBar.UpdateFill(health.Percentage);
    }
}