using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(AmmoDropper))]
[RequireComponent(typeof(AmmoInventory))]
public class DropInput : MonoBehaviourPun
{
    [Header("Drop Settings")]
    [SerializeField] private KeyCode dropKey = KeyCode.G;
    [SerializeField] private int dropAmount = 10;

    private AmmoDropper ammoDropper;
    private AmmoInventory ammoInventory;
    
    private void Awake()
    {
        ammoDropper = GetComponent<AmmoDropper>();
        ammoInventory = GetComponent<AmmoInventory>();
    }
    
    private void Update()
    {
        if (!photonView.IsMine) return; // solo el jugador local controla su input

        if (Input.GetKeyDown(dropKey))
        {
            HandleDrop();
        }
    }
    
    private void HandleDrop()
    {
        // por ahora, dropea siempre el tipo de munición del arma equipada
        if (TryGetComponent(out WeaponController weaponController) && weaponController.currentWeapon != null)
        {
            AmmoType currentAmmoType = weaponController.currentWeapon.ammoType;
            int currentAmmo = ammoInventory.GetAmmo(currentAmmoType);

            if (currentAmmo > 0)
            {
                int drop = Mathf.Min(dropAmount, currentAmmo);
                ammoDropper.DropAmmo(currentAmmoType, drop);
                Debug.Log($"Dropped {drop} of {currentAmmoType}");
            }
            else
            {
                Debug.Log("No ammo to drop");
            }
        }
    }
}
