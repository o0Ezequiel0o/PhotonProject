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

    public Weapon currentWeapon = null;

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

        return true;
    }

    private void Update()
    {
        if (currentWeapon == null) return;

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