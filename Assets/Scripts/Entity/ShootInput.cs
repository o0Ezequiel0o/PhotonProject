using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ShootInput : MonoBehaviour
{
    [Header("Dependency")]
    [SerializeField] private Player player;
    [SerializeField] private PhotonView photonView;
    [SerializeField] private WeaponController weaponController;
    [SerializeField] private Transform fireOrigin;
    
    [Header("Settings")]
    [SerializeField] private KeyCode fireKey = KeyCode.Mouse0;
    

    void Reset()
    {
        photonView = GetComponent<PhotonView>();
        weaponController = GetComponent<WeaponController>();
        fireOrigin = transform;
    }
    
    void Update()
    {
        if (!photonView.IsMine) return;
        if (player.IsDowned) return;

        if (weaponController == null || weaponController.currentWeapon == null) return;

        if (Input.GetKey(fireKey))
        {
            weaponController.currentWeapon.Fire(gameObject);

        }
    }
}
