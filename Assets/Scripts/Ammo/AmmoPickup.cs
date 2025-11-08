using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AmmoPickup : MonoBehaviourPun, IInteractable
{
    public AmmoType ammoType;
    public int amount = 10;

    public bool Interact(GameObject source)
    {
        if (!photonView.IsMine) return false;

        if (source.TryGetComponent(out AmmoInventory inventory))
        {
            inventory.AddAmmo(ammoType, amount);
            PhotonNetwork.Destroy(gameObject);
            return true;
        }

        return false;
    }
}
