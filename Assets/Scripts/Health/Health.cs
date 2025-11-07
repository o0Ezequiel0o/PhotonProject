using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Health : MonoBehaviourPun
{
    [SerializeField] private bool destroyOnDeath = true;

    public float maxHp = 100f;
    public float currentHp;

    public Team team = Team.None;

    public Action onDamageTaken;
    public Action onDeath;

    public float Percentage => currentHp / maxHp;

    private void Reset()
    {
        currentHp = maxHp;
    }

    private void Start()
    {
        if (currentHp <= 0f)
        {
            currentHp = maxHp;
        }
    }

    [PunRPC]
    public void RPC_UpdateHealth(int newValue)
    {
        currentHp = Mathf.Min(newValue, maxHp);
    }

    [PunRPC]
    public void RPC_TakeDamage(int amount, int attackerActorNumber, Team attackerTeam)
    {
        //if (attackerTeam == team) return;
        
        currentHp -= amount;
        Debug.Log($"{gameObject.name} ({team}) took {amount} dmg from Actor {attackerActorNumber}. HP now: {currentHp}");

        onDamageTaken?.Invoke();

        if (currentHp <= 0)
        {
            if (destroyOnDeath)
            {
                Die(attackerActorNumber);
            }
            onDeath?.Invoke();
        }
    }

    [PunRPC]
    public void RPC_TakeDamageAny(int amount)
    {
        //if (attackerTeam == team) return;

        int attackerActorNumber = 0;

        currentHp -= amount;
        Debug.Log($"{gameObject.name} ({team}) took {amount} dmg from Actor {attackerActorNumber}. HP now: {currentHp}");

        onDamageTaken?.Invoke();

        if (currentHp <= 0)
        {
            if (destroyOnDeath)
            {
                photonView.RPC("RPC_OnDeathAll", RpcTarget.AllBuffered, attackerActorNumber);
            }
            onDeath?.Invoke();
        }
    }

    void Die(int attackerActorNumber)
    {
        Debug.Log($"{gameObject.name} ({team}) died. Killed by Actor {attackerActorNumber}");
        
        photonView.RPC("RPC_OnDeathAll", RpcTarget.AllBuffered, attackerActorNumber);
    }
    
    [PunRPC]
    void RPC_OnDeathAll(int attackerActorNumber)
    {
        gameObject.SetActive(false);
    }
}
