using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameOverController : MonoBehaviourPunCallbacks
{
    public static GameOverController Instance;
    public Action onGameOver;

    private bool gameOverTriggered = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (gameOverTriggered) return;

        CheckPlayersDowned();
    }

    private void CheckPlayersDowned()
    {
        Player[] players = FindObjectsOfType<Player>();
        if (players.Length == 0) return;

        foreach (var p in players)
        {
            if (!p.IsDowned)
                return;
        }

        gameOverTriggered = true;

        CloseRoom();

        photonView.RPC(nameof(RPC_GameOver), RpcTarget.AllBuffered);
    }
    
    private void CloseRoom()
    {
        if (!PhotonNetwork.IsMasterClient || PhotonNetwork.CurrentRoom == null)
            return;

        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
    }

    [PunRPC]
    void RPC_GameOver()
    {
        LocalGameOverController.Instance.ShowGameOverScreen();
        onGameOver?.Invoke();
    }
}
