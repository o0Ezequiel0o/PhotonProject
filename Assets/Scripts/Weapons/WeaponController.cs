using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] List<WeaponSlot> weaponSlots;
    [SerializeField] private Transform grabPosition;
    [SerializeField] private Transform pivot;
    [SerializeField] public PhotonView photonView;
    [SerializeField] private Player player;

    public Weapon currentWeapon = null;

    public event Action OnWeaponChanged;

    public bool GrabWeapon(Weapon pickedWeapon)
    {
        if (!photonView.IsMine) return false; // solo el jugador local puede equipar
        if (pickedWeapon.equipped) return false;
        if (currentWeapon != null) return false;

        // Buscar el arma equivalente dentro del prefab local del jugador
        Weapon weapon = GetWeaponInHierarchy(pickedWeapon.Name);
        if (weapon == null)
        {
            Debug.LogWarning($"No se encontró el arma {pickedWeapon.Name} en el jugador local.");
            return false;
        }

        // Marcarla como equipada localmente
        currentWeapon = weapon;
        currentWeapon.gameObject.SetActive(true);
        currentWeapon.equipped = true;

        // Destruir el arma física del mapa (pickup)
        if (pickedWeapon.photonView != null && pickedWeapon.photonView.IsMine)
        {
            pickedWeapon.photonView.RPC("RPC_DestroyWeapon", RpcTarget.AllBuffered);
        }
        else
        {
            PhotonNetwork.Destroy(pickedWeapon.gameObject);
        }

        // Sincronizar el arma seleccionada con los demás jugadores
        photonView.RPC(nameof(RPC_SetEquippedWeapon), RpcTarget.AllBuffered, weapon.Name);

        OnWeaponChanged?.Invoke();

        return true;
    }
    
    [PunRPC]
    private void RPC_SetEquippedWeapon(string weaponName)
    {
        Weapon weapon = GetWeaponInHierarchy(weaponName);
        if (weapon == null) return;

        if (currentWeapon != null)
            currentWeapon.gameObject.SetActive(false);

        currentWeapon = weapon;
        currentWeapon.gameObject.SetActive(true);
        currentWeapon.equipped = true;

        OnWeaponChanged?.Invoke();
    }

    private void Update()
    {
        if (currentWeapon == null) return;

        if (player.IsDowned) //use events instead of constantly sending info
        {
            photonView.RPC("RPC_HideWeapon", RpcTarget.AllBuffered, currentWeapon.Name);
        }
        else
        {
            photonView.RPC("RPC_ShowWeapon", RpcTarget.AllBuffered, currentWeapon.Name);
        }

        currentWeapon.transform.position = grabPosition.position;
        currentWeapon.transform.rotation = pivot.rotation;
    }

    private Weapon GetWeaponInHierarchy(string weapon)
    {
        foreach (Transform children in grabPosition.transform)
        {
            Debug.Log(children.name);
            if (children.name == weapon)
            {
                return children.GetComponent<Weapon>();
            }
        }

        return null;
    }

    [PunRPC]
    public void RPC_HideWeapon(string weapon)
    {
        foreach (Transform children in grabPosition.transform)
        {
            if (children.name == weapon)
            {
                children.gameObject.SetActive(false);
            }
        }
    }

    [PunRPC]
    public void RPC_ShowWeapon(string weapon)
    {
        foreach (Transform children in grabPosition.transform)
        {
            if (children.name == weapon)
            {
                children.gameObject.SetActive(true);
            }
        }
    }

    [PunRPC]
    public void RPC_EquipWeapon(string weapon)
    {
        foreach (Transform children in grabPosition.transform)
        {
            if (children.name == weapon)
            {
                children.gameObject.SetActive(true);
            }
        }
    }

    [PunRPC]
    public void RPC_UnequipCurrent() { }

    [Serializable]
    public struct WeaponSlot
    {
        [SerializeField] private string name;
        [SerializeField] private GameObject weapon;
    }
}