using Photon.Pun;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IInteractable
{
    public PhotonView photonView;

    public bool equipped = false;
    
    public string projectilePrefabName = "";
    public float projectileSpeed = 10f;
    public int damage = 10;

    public virtual void Fire(GameObject source) { }

    public bool Interact(GameObject source)
    {
        if (source.TryGetComponent(out WeaponController weaponController))
        {
            return weaponController.GrabWeapon(this);
        }

        return false;
    }
}