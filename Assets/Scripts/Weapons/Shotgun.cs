using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Shotgun : Weapon
{
    [Header("Shotgun Settings")]
    public int pelletCount = 6;         
    public float spreadAngle = 15f;     
    public float recoilForce = 2f; 
    
    public override void Fire(GameObject source)
    {
        if (string.IsNullOrEmpty(projectilePrefabName))
        {
            Debug.LogWarning("ShotgunWeapon: projectilePrefabName empty");
            return;
        }
        
        if (source.TryGetComponent(out AmmoInventory ammoInv))
        {
            if (!ammoInv.ConsumeAmmo(ammoType, ammoPerShot))
            {
                Debug.Log("No ammo!");
                return;
            }
        }

        Vector3 spawnPos = transform.position;
        Quaternion baseRot = transform.rotation;

        // Determinar el equipo del tirador
        Team shooterTeam = Team.None;
        if (source.TryGetComponent(out Health sourceHealth))
        {
            shooterTeam = sourceHealth.team;
        }

        // Instanciar múltiples proyectiles con ángulo aleatorio
        for (int i = 0; i < pelletCount; i++)
        {
            // calcular un ángulo de dispersión
            float randomSpread = Random.Range(-spreadAngle / 2f, spreadAngle / 2f);
            Quaternion spreadRot = baseRot * Quaternion.Euler(0, 0, randomSpread);
            Vector2 dir = spreadRot * Vector2.up; // dirección final del pellet

            object[] instantiationData = new object[]
            {
                PhotonNetwork.LocalPlayer.ActorNumber,
                dir.x,
                dir.y,
                damage,
                projectileSpeed,
                shooterTeam
            };

            PhotonNetwork.Instantiate(
                projectilePrefabName,
                spawnPos,
                spreadRot,
                0,
                instantiationData
            );
        }
        
        // Aplicar retroceso al jugador (opcional)
        if (source.TryGetComponent(out Rigidbody2D rb))
        {
            rb.AddForce(-transform.up * recoilForce, ForceMode2D.Impulse);
        }
        
    }
}
