using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class HealthbarHandler : MonoBehaviour
{
    [SerializeField] private Transform healthBarRoot;
    [SerializeField] private Image healthBarFill;

    private Health health;

    private void Awake()
    {
        if (TryGetComponent(out health))
        {
            health.onDamageTaken += UpdateHealthBar;
        }
    }

    private void LateUpdate()
    {
        healthBarRoot.transform.rotation = Quaternion.identity;
    }

    private void UpdateHealthBar()
    {
        gameObject.GetPhotonView().RPC("RPC_UpdateHealthBar", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void RPC_UpdateHealthBar()
    {
        healthBarFill.fillAmount = health.currentHp / health.maxHp;
    }
}