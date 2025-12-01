using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Projectile : MonoBehaviourPun
{
    private Rigidbody2D rb;
    private int ownerActorNumber = -1;
    private Team attackerTeam = Team.None;
    private Vector2 direction = Vector2.up;
    private float speed = 10f;
    public float lifetime = 5f;
    private int damage = 10;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        object[] data = photonView.InstantiationData;
        if (data != null && data.Length >= 6)
        {
            // data: [ownerActorNumber, dir.x, dir.y, damage, speed]
            ownerActorNumber = (int)data[0];
            float dx = Convert.ToSingle(data[1]);
            float dy = Convert.ToSingle(data[2]);
            damage = Convert.ToInt32(data[3]);
            speed = Convert.ToSingle(data[4]);
            attackerTeam = (Team)(int)data[5];

            direction = new Vector2(dx, dy).normalized;
        }
        
        rb.velocity = direction * speed;
        
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == this.gameObject) return;

        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent(out PhotonView targetPv) &&
                other.TryGetComponent(out Health targetHealth))
            {
                //if (targetHealth.team == attackerTeam) return;

                if (other.CompareTag("Player"))
                {
                    if (targetPv.Owner.ActorNumber == ownerActorNumber) return;
                }
                
                targetPv.RPC("RPC_TakeDamage", RpcTarget.AllBuffered, damage, ownerActorNumber, attackerTeam);

                if (photonView.IsMine)
                {
                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }

        else if (other.CompareTag("Obstacle"))
        {
            if (photonView.IsMine)
            {
                PhotonNetwork.Instantiate("ObstacleHitEffect", transform.position, Quaternion.identity);
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
