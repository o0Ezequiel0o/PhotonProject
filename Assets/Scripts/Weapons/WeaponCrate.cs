using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class WeaponCrate : MonoBehaviourPun, IInteractable
{
    [Header("Weapon Loot Settings")]
    [SerializeField] private List<string> weaponNames; // Lista de armas posibles
    [SerializeField] private int ammoRewardOnDuplicate = 200;
    
    [Header("FX & Sound (opcional)")]
    [SerializeField] private GameObject openEffect;
    [SerializeField] private AudioClip openSound;

    private bool isOpened = false;
    
    public bool Interact(GameObject source)
    {
        if (isOpened) return false;

        if (!source.TryGetComponent(out WeaponController weaponController))
            return false;
        
        string randomName = weaponNames[Random.Range(0, weaponNames.Count)];
        
        if (weaponController.currentWeapon != null &&
            weaponController.currentWeapon.Name == randomName)
        {
            if (source.TryGetComponent(out AmmoInventory inv))
            {
                inv.AddAmmo(weaponController.currentWeapon.ammoType, ammoRewardOnDuplicate);
                Debug.Log($"[Crate] Recargaste munición (+{ammoRewardOnDuplicate}) para {randomName}");
            }

            OpenAndDestroyCrate();
            return true;
        }
        
        Weapon[] playerWeapons = source.GetComponentsInChildren<Weapon>(true);
        Weapon targetWeapon = null;

        foreach (Weapon w in playerWeapons)
        {
            if (w.Name == randomName)
            {
                targetWeapon = w;
                break;
            }
        }

        if (targetWeapon == null)
        {
            Debug.LogWarning($"[Crate] El jugador no tiene el arma {randomName} entre sus prefabs internos.");
            return false;
        }

        // Equipar arma
        
        Debug.Log($"[Crate] Nueva arma equipada: {randomName}");

        OpenAndDestroyCrate();
        // Desactivar el arma actual
        if (weaponController.currentWeapon != null) {
            weaponController.currentWeapon.gameObject.SetActive(false);
            weaponController.currentWeapon.equipped = false;
        }

// Activar la nueva
        targetWeapon.gameObject.SetActive(true);
        targetWeapon.equipped = true;
        weaponController.currentWeapon = targetWeapon;

// Sincronizar visual con los demás jugadores
        weaponController.photonView.RPC(
            "RPC_SetEquippedWeapon",
            RpcTarget.AllBuffered,
            targetWeapon.Name
        );

        return true;
    }

    private void OpenAndDestroyCrate()
    {
        isOpened = true;
        
        if (openEffect)
            Instantiate(openEffect, transform.position, Quaternion.identity);

        if (openSound)
            AudioSource.PlayClipAtPoint(openSound, transform.position);
        
        if (photonView != null)
        {
            photonView.RPC(nameof(RPC_DestroyCrate), RpcTarget.AllBuffered);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [PunRPC]
    private void RPC_DestroyCrate()
    {
        // if (photonView != null && photonView.IsMine)
        //     PhotonNetwork.Destroy(gameObject);
        // else
        //     Destroy(gameObject);

        if (!PhotonNetwork.IsMasterClient) return;

        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
    
    [PunRPC]
    private void RPC_OpenCrateFX()
    {
        if (openEffect != null)
            Instantiate(openEffect, transform.position, Quaternion.identity);
        if (openSound != null)
            AudioSource.PlayClipAtPoint(openSound, transform.position);
        
        Destroy(gameObject);
    }
    
    private GameObject GetPlayerObject(Photon.Realtime.Player player)
    {
        foreach (var go in GameObject.FindGameObjectsWithTag("Player"))
        {
            PhotonView view = go.GetComponent<PhotonView>();
            if (view != null && view.Owner == player)
                return go;
        }
        return null;
    }

}
