using UnityEngine;
using Photon.Pun;

public class IncreaseKillCount : MonoBehaviour
{
    private void Awake()
    {
        if (TryGetComponent(out Health health))
        {
            health.onDeath += IncreaseKillCountPhoton;
        }
    }

    private void IncreaseKillCountPhoton()
    {
        Debug.Log("zombie killed");
        gameObject.GetPhotonView().RPC("RPC_IncreaseKillCount", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void RPC_IncreaseKillCount()
    {
        KillsManager.totalKills += 1;
    }
}