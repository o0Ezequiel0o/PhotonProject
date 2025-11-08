using UnityEngine;
using Photon.Pun;

public abstract class Weapon : MonoBehaviourPun, IInteractable
{
    public string Name = "";

    public Sprite icon;

    public bool equipped = false;
    
    public string projectilePrefabName = "";
    public float projectileSpeed = 10f;
    public int damage = 10;
    
    public AmmoType ammoType = AmmoType.Default;
    public int ammoPerShot = 1;

    public virtual void Fire(GameObject source) { }

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