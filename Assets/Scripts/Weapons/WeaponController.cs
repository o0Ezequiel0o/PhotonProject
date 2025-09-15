using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Transform grabPosition;
    [SerializeField] private Transform pivot;
    [SerializeField] private PhotonView PhotonView;
    [SerializeField] private int maxWeapons = 1;

    private List<Weapon> weapons = new List<Weapon>();
    public Weapon currentWeapon = null;

    public bool GrabWeapon(Weapon weapon)
    {
        if (weapon.equipped) return false;

        if (currentWeapon != null || weapons.Count >= maxWeapons) return false;

        PhotonView targetPv = weapon.GetComponent<PhotonView>();

        targetPv.RPC("RPC_OnEquip", RpcTarget.All, true);
        weapon.photonView.TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);

        currentWeapon = weapon;

        weapon.transform.rotation = Quaternion.identity;
        return true;
    }

    private void Update()
    {
        if (currentWeapon == null) return;

        currentWeapon.transform.position = grabPosition.position;
        currentWeapon.transform.rotation = pivot.rotation;
    }
}