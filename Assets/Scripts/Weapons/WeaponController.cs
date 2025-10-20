using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] List<WeaponSlot> weaponSlots;
    [SerializeField] private Transform grabPosition;
    [SerializeField] private Transform pivot;
    [SerializeField] private PhotonView PhotonView;

    public Weapon currentWeapon = null;

    public bool GrabWeapon(Weapon pickedWeapon)
    {
        if (pickedWeapon.equipped) return false;

        if (currentWeapon != null) return false;

        Weapon weapon = GetWeaponInHierarchy(pickedWeapon.Name);

        currentWeapon = weapon;

        weapon.GetComponent<PhotonView>().RPC("RPC_OnEquip", RpcTarget.All, true, "");
        pickedWeapon.GetComponent<PhotonView>().RPC("RPC_DestroyWeapon", RpcTarget.All);
        GetComponent<PhotonView>().RPC("RPC_EquipWeapon", RpcTarget.All, weapon.Name);

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
        Debug.Log(weapon);

        foreach (Transform children in grabPosition.transform)
        {
            Debug.Log(children.name);
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