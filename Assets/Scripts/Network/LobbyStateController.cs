using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LobbyStateController : MonoBehaviourPunCallbacks
{
    [SerializeField] private static Dictionary<int, Action> buffer = new Dictionary<int, Action>();

    public void BufferRPC(int viewID, Action callBack)
    {

    }
}