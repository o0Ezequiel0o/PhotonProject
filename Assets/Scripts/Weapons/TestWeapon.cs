using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TestWeapon : Weapon
{
    public override void Fire(GameObject source)
    {
        if (string.IsNullOrEmpty(projectilePrefabName))
        {
            Debug.LogWarning("TestWeapon: projectilePrefabName empty");
            return;
        }
        
        Vector3 spawnPos = transform.position;
        Quaternion spawnRot = transform.rotation;
        
        Vector2 dir = transform.up.normalized;
        
        object[] instantiationData = new object[]
        {
            PhotonNetwork.LocalPlayer.ActorNumber,
            dir.x,
            dir.y,
            damage,
            projectileSpeed
        };
        
        GameObject proj = PhotonNetwork.Instantiate(projectilePrefabName, spawnPos, spawnRot, 0, instantiationData);
    }
}