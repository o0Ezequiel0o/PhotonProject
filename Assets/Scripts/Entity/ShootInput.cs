using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ShootInput : MonoBehaviour
{
    [Header("Dependency")]
    [SerializeField] private PhotonView photonView;
    [SerializeField] private WeaponController weaponController;
    [SerializeField] private Transform fireOrigin;
    
    [Header("Settings")]
    [SerializeField] private float fireRate = 5f;
    [SerializeField] private KeyCode fireKey = KeyCode.Mouse0;
    
    private float fireCooldown = 0f;

    void Reset()
    {
        photonView = GetComponent<PhotonView>();
        weaponController = GetComponent<WeaponController>();
        fireOrigin = transform;
    }
    
    void Update()
    {
        if (!photonView.IsMine) return;
        if (weaponController == null || weaponController.currentWeapon == null) return;

        fireCooldown -= Time.deltaTime;
        if (Input.GetKey(fireKey) && fireCooldown <= 0f)
        {
            Vector2 dir;
            if (weaponController.currentWeapon.transform != null)
                dir = weaponController.currentWeapon.transform.up;
            else
                dir = transform.up;

            weaponController.currentWeapon.Fire(gameObject);

            fireCooldown = 1f / fireRate;
        }
    }
}
