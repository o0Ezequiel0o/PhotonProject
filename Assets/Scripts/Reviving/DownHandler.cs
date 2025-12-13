using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Photon.Pun;

public class DownHandler : MonoBehaviour, IInteractable
{
    [SerializeField] private PhotonView photonView;
    [Space]
    [SerializeField] private float reviveTime;
    [SerializeField] private float stopRevivingDistance;

    public bool IsDowned = false;
    private bool beingRevived = false;
    private Player reviver = null;

    private StatMultiplier downedSpeedMultiplier = new StatMultiplier(0f);
    private StatMultiplier revivingSpeedMultiplier = new StatMultiplier(1f);
    private Sprite originalSprite;

    private GameObject reviveBarInstance;

    private Coroutine reviveCoroutine;

    public bool Interact(GameObject source)
    {
        if (source == gameObject) return false;

        if (IsDowned)
        {
            reviver = source.GetComponent<Player>();
            reviveCoroutine = StartCoroutine(ReviveCoroutine());
            beingRevived = true;

            return true;
        }

        return false;
    }

    private void Update()
    {
        if (!beingRevived) return;

        if (Vector3.Distance(reviver.transform.position, transform.position) > stopRevivingDistance)
        {
            StopCoroutine(reviveCoroutine);

            if (reviveBarInstance != null)
            {
                reviveBarInstance.GetPhotonView().RPC("RPC_DestroyBar", RpcTarget.All);
            }
        }
    }

    private IEnumerator ReviveCoroutine()
    {
        reviveBarInstance = PhotonNetwork.Instantiate("ReviveBar", transform.position, Quaternion.identity);
        reviveBarInstance.GetPhotonView().RPC("RPC_StartBar", RpcTarget.All, reviveTime);
        reviver.gameObject.GetPhotonView().RPC("RPC_StartRevive", RpcTarget.All);
        Debug.Log("start reviving");
        yield return new WaitForSeconds(reviveTime);
        reviver.gameObject.GetPhotonView().RPC("RPC_EndRevive", RpcTarget.All);
        Revive();
    }

    private void Awake()
    {
        if (TryGetComponent(out Health health))
        {
            health.onDeath += GoDowned;
        }

        if (TryGetComponent(out EntityMove entityMove))
        {
            entityMove.moveSpeed.AddMultiplier(revivingSpeedMultiplier);
        }
    }

    private void Revive()
    {
        photonView.RPC("RPC_Revive", RpcTarget.All);
        photonView.RPC("RPC_UpdateHealth", RpcTarget.All, 100);

        if (reviveBarInstance != null)
        reviveBarInstance.GetPhotonView().RPC("RPC_DestroyBar", RpcTarget.All);
    }

    private void GoDowned()
    {
        photonView.RPC("RPC_GoDowned", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_StartRevive()
    {
        revivingSpeedMultiplier.UpdateMultiplier(0f);
    }

    [PunRPC]
    private void RPC_EndRevive()
    {
        revivingSpeedMultiplier.UpdateMultiplier(1f);
    }

    [PunRPC]
    private void RPC_Revive()
    {
        IsDowned = false;

        if (TryGetComponent(out EntityMove entityMove))
        {
            entityMove.moveSpeed.RemoveMultiplier(downedSpeedMultiplier);
        }

        Debug.Log("revived");
    }

    [PunRPC]
    private void RPC_GoDowned()
    {
        if (IsDowned) return;

        IsDowned = true;

        if (TryGetComponent(out EntityMove entityMove))
        {
            entityMove.moveSpeed.AddMultiplier(downedSpeedMultiplier);
        }
    }
}