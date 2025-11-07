using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Explosion : MonoBehaviour
{
    [SerializeField] private int explosionDamage = 30;
    [SerializeField] private float explosionRange = 3f;
    [Space]
    [SerializeField] private bool destroyObject = false;

    private bool exploded = false;

    private void Awake()
    {
        if (TryGetComponent(out Health health))
        {
            health.onDeath += TriggerExplosion;
        }
    }

    private void TriggerExplosion()
    {
        if (!exploded)
        {
            gameObject.GetPhotonView().RPC("RPC_TriggerExplosion", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void RPC_TriggerExplosion()
    {
        exploded = true;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRange);

        foreach(Collider2D hit in hits)
        {
            if (hit.gameObject == gameObject) continue;

            if (hit.TryGetComponent(out Health health))
            {
                health.gameObject.GetPhotonView().RPC("RPC_TakeDamageAny", RpcTarget.All, explosionDamage);
            }
        }

        if (destroyObject)
        {
            Destroy(gameObject);
        }
    }
}
