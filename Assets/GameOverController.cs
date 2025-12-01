using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameOverController : MonoBehaviourPunCallbacks
{
    public static GameOverController Instance;
    public Action onGameOver;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        CheckPlayersDowned();
    }

    private void CheckPlayersDowned()
    {
        Player[] players = FindObjectsOfType<Player>();

        bool allDowned = true;

        foreach (var p in players)
        {
            if (!p.IsDowned)
            {
                allDowned = false;
                break;
            }
        }

        if (allDowned)
        {
            photonView.RPC("RPC_GameOver", RpcTarget.All);
        }
    }

    [PunRPC]
    void RPC_GameOver()
    {
        LocalGameOverController.Instance.ShowGameOverScreen();
        onGameOver?.Invoke();
    }
}
