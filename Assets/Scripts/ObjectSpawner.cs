using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private List<Spawnable> spawn = new List<Spawnable>();

    void Awake()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        foreach(Spawnable thingToSpawn in spawn)
        {
            PhotonNetwork.InstantiateRoomObject(thingToSpawn.name, thingToSpawn.position, Quaternion.identity);
        }
    }

    [Serializable]
    private struct Spawnable
    {
        public string name;
        public Vector3 position;
        public Quaternion rotation;
    }
}
