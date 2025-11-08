using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviourPun
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private Image weaponIcon;

    private AmmoInventory ammoInventory;
    private WeaponController weaponController;
    
    
    public void Initialize(AmmoInventory inventory, WeaponController controller)
    {
        ammoInventory = inventory;
        weaponController = controller;

        weaponController.OnWeaponChanged += UpdateAmmoUI;
        UpdateAmmoUI();
    }
    
    private void OnDestroy()
    {
        if (weaponController != null)
        {
            weaponController.OnWeaponChanged -= UpdateAmmoUI;
        }
    }
    
    private void Update()
    {
        // Actualiza en tiempo real el número de munición
        UpdateAmmoUI();
    }
    
    private void UpdateAmmoUI()
    {
        if (weaponController == null || weaponController.currentWeapon == null)
        {
            ammoText.text = "- / -";
            if (weaponIcon) weaponIcon.enabled = false;
            return;
        }

        if (weaponIcon)
        {
            weaponIcon.enabled = true;
            weaponIcon.sprite = weaponController.currentWeapon.icon; // opcional si le agregás un Sprite a Weapon
        }

        AmmoType currentAmmo = weaponController.currentWeapon.ammoType;

        int totalAmmo = ammoInventory != null ? ammoInventory.GetAmmo(currentAmmo) : 0;
        int ammoPerShot = weaponController.currentWeapon.ammoPerShot;

        ammoText.text = $"{totalAmmo}";
    }
}
