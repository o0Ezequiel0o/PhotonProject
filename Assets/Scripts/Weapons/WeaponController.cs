using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] List<WeaponSlot> weaponSlots;
    [SerializeField] private Transform grabPosition;
    [SerializeField] private Transform pivot;
    [SerializeField] private PhotonView photonView;
    [SerializeField] private Player player;

    public Weapon currentWeapon = null;

    public event Action OnWeaponChanged;

    public bool GrabWeapon(Weapon pickedWeapon)
    {
        if (pickedWeapon.equipped) return false;

        if (currentWeapon != null) return false;

        Weapon weapon = GetWeaponInHierarchy(pickedWeapon.Name);

        currentWeapon = weapon;

        PhotonView weaponPV = weapon.GetComponent<PhotonView>();
        PhotonView pickedWeaponPV = pickedWeapon.GetComponent<PhotonView>();

        weaponPV.RPC("RPC_OnEquip", RpcTarget.All, true, "");
        pickedWeaponPV.RPC("RPC_DestroyWeapon", RpcTarget.All);
        photonView.RPC("RPC_EquipWeapon", RpcTarget.All, weapon.Name);
        
        OnWeaponChanged?.Invoke();

        return true;
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