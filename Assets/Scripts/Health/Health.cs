using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Health : MonoBehaviourPun
{
    public int maxHp = 100;
    public int currentHp;

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
    public void RPC_TakeDamage(int amount, int attackerActorNumber)
    {
        currentHp -= amount;
        Debug.Log($"Player {photonView.OwnerActorNr} took {amount} dmg from {attackerActorNumber}. HP now: {currentHp}");

        if (currentHp <= 0)
        {
            Die(attackerActorNumber);
        }
    }

    void Die(int attackerActorNumber)
    {
        Debug.Log($"Player {photonView.OwnerActorNr} died. Killed by {attackerActorNumber}");
        
        photonView.RPC("RPC_OnDeathAll", RpcTarget.AllBuffered, attackerActorNumber);
    }
    
    [PunRPC]
    void RPC_OnDeathAll(int attackerActorNumber)
    {
        gameObject.SetActive(false);
    }
}
