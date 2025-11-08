using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AmmoInventory : MonoBehaviourPun
{
    [Header("Ammo Config")]
    public Dictionary<AmmoType, int> ammoDict = new Dictionary<AmmoType, int>();

    [Header("Default Ammo")] 
    public int defaultAmmo = 100;
    public int defaultPistolAmmo = 30;
    public int defaultShotgunAmmo = 10;
    public int defaultRifleAmmo = 50;

    private void Awake()
    {
        // inicializamos munición base
        ammoDict[AmmoType.Default] = defaultAmmo;
        ammoDict[AmmoType.Pistol] = defaultPistolAmmo;
        ammoDict[AmmoType.Shotgun] = defaultShotgunAmmo;
        ammoDict[AmmoType.Rifle] = defaultRifleAmmo;
    }
    
    public bool HasAmmo(AmmoType type)
    {
        return ammoDict.ContainsKey(type) && ammoDict[type] > 0;
    }
    
    public bool ConsumeAmmo(AmmoType type, int amount)
    {
        if (!HasAmmo(type)) return false;

        ammoDict[type] = Mathf.Max(0, ammoDict[type] - amount);

        photonView.RPC(nameof(RPC_SyncAmmo), RpcTarget.Others, (int)type, ammoDict[type]);
        Debug.Log($"Consumed {amount} of {type}, remaining: {ammoDict[type]}");
        return true;
    }
    
    public void AddAmmo(AmmoType type, int amount)
    {
        if (!ammoDict.ContainsKey(type))
            ammoDict[type] = 0;

        ammoDict[type] += amount;

        photonView.RPC(nameof(RPC_SyncAmmo), RpcTarget.Others, (int)type, ammoDict[type]);
    }
    
    [PunRPC]
    private void RPC_SyncAmmo(int type, int value)
    {
        ammoDict[(AmmoType)type] = value;
    }

    public int GetAmmo(AmmoType type)
    {
        return ammoDict.ContainsKey(type) ? ammoDict[type] : 0;
    }
}
