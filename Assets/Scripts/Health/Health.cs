using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Health : MonoBehaviourPun
{
    public int maxHp = 100;
    public int currentHp;

    public Team team = Team.None;

    private void Reset()
    {
        currentHp = maxHp;
    }

    private void Start()
    {
        if (currentHp <= 0)
        {
            currentHp = maxHp;
        }
    }

    [PunRPC]
    public void RPC_TakeDamage(int amount, int attackerActorNumber, Team attackerTeam)
    {
        if (attackerTeam == team) return;
        
        currentHp -= amount;
        Debug.Log($"{gameObject.name} ({team}) took {amount} dmg from Actor {attackerActorNumber}. HP now: {currentHp}");

        if (currentHp <= 0)
        {
            Die(attackerActorNumber);
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
