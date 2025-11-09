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
        // 🔸 Prevención de doble uso
        if (isOpened) return false;

        if (!source.TryGetComponent(out WeaponController weaponController))
            return false;

        // 🔸 Elegir un arma random
        string randomName = weaponNames[Random.Range(0, weaponNames.Count)];

        // 🔸 Si ya tiene esa arma → dar munición
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

        // 🔸 Buscar entre las armas del jugador (desactivadas dentro del prefab)
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

        // 🔸 Equipar arma
        weaponController.GrabWeapon(targetWeapon);
        Debug.Log($"[Crate] Nueva arma equipada: {randomName}");

        OpenAndDestroyCrate();
        return true;
    }

    private void OpenAndDestroyCrate()
    {
        isOpened = true;

        // 🔹 Mostrar efectos visuales y de sonido localmente
        if (openEffect)
            Instantiate(openEffect, transform.position, Quaternion.identity);

        if (openSound)
            AudioSource.PlayClipAtPoint(openSound, transform.position);

        // 🔹 Destruir la caja en red
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
        if (photonView != null && photonView.IsMine)
            PhotonNetwork.Destroy(gameObject);
        else
            Destroy(gameObject);
    }
    
    // [PunRPC]
    // private void RPC_RequestOpen(int playerActorNumber)
    // {
    //     if (isOpened) return;
    //
    //     // Solo el MasterClient ejecuta la lógica
    //     Photon.Realtime.Player player = PhotonNetwork.CurrentRoom.GetPlayer(playerActorNumber);
    //     if (player == null) return;
    //
    //     GameObject playerObj = GetPlayerObject(player);
    //     if (playerObj == null) return;
    //
    //     if (!playerObj.TryGetComponent(out WeaponController weaponController)) return;
    //     if (!playerObj.TryGetComponent(out AmmoInventory ammoInventory)) return;
    //
    //     int randomIndex = Random.Range(0, weaponPrefabs.Length);
    //     GameObject randomWeaponPrefab = weaponPrefabs[randomIndex];
    //     Weapon randomWeapon = randomWeaponPrefab.GetComponent<Weapon>();
    //
    //     if (randomWeapon == null) return;
    //
    //     Weapon currentWeapon = weaponController.currentWeapon;
    //     if (currentWeapon != null && currentWeapon.Name == randomWeapon.Name)
    //     {
    //         ammoInventory.AddAmmo(randomWeapon.ammoType, ammoRewardOnDuplicate);
    //         Debug.Log($"Duplicado: +{ammoRewardOnDuplicate} de munición para {randomWeapon.Name}");
    //     }
    //     else
    //     {
    //         weaponController.GrabWeapon(randomWeapon.GetComponent<Weapon>());
    //         Debug.Log($"Nueva arma: {randomWeapon.Name}");
    //     }
    //
    //     photonView.RPC(nameof(RPC_OpenCrateFX), RpcTarget.All);
    //     isOpened = true;
    // }
    
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
