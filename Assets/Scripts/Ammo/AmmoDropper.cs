using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


public class AmmoDropper : MonoBehaviourPun
{
    [SerializeField] private GameObject ammoPickupPrefab;

    public void DropAmmo(AmmoType type, int amount)
    {
        if (!photonView.IsMine) return;

        if (TryGetComponent(out AmmoInventory inv))
        {
            if (inv.GetAmmo(type) < amount)
            {
                Debug.Log("Not enough ammo to drop");
                return;
            }

            inv.ConsumeAmmo(type, amount);
            
            Vector3 dropPos = transform.position + transform.up * 0.5f;
            GameObject pickup = PhotonNetwork.Instantiate(ammoPickupPrefab.name, dropPos, Quaternion.identity);
            pickup.GetComponent<AmmoPickup>().ammoType = type;
            pickup.GetComponent<AmmoPickup>().amount = amount;
        }
    }
}
