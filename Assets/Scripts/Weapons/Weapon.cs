using UnityEngine;
using Photon.Pun;

public abstract class Weapon : MonoBehaviourPun, IInteractable
{
    [Header("Weapon Settings")]
    public string Name = "";
    public Sprite icon;
    public bool equipped = false;
    
    [Header("Stats")]
    public string projectilePrefabName = "";
    public float projectileSpeed = 10f;
    public int damage = 10;
    public float fireRate = 5f;
    
    [Header("Ammo Settings")]
    public AmmoType ammoType = AmmoType.Default;
    public int ammoPerShot = 1;

    private float nextFireTime = 0f;

    public virtual void Fire(GameObject source)
    {
        if (Time.time < nextFireTime) return;

        nextFireTime = Time.time + (1f / fireRate);

        PerformShoot(source);
    }
    
    protected abstract void PerformShoot(GameObject source);

    public bool Interact(GameObject source)
    {
        if (source.TryGetComponent(out WeaponController weaponController))
        {
            return weaponController.GrabWeapon(this);
        }

        return false;
    }

    [PunRPC]
    public void RPC_OnEquip(bool equipped, string weapon)
    {
        this.equipped = equipped;
    }

    [PunRPC]
    public void RPC_DestroyWeapon()
    {
        Destroy(gameObject);
    }
}