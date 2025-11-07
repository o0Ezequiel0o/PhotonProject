using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private List<Spawnable> spawn = new List<Spawnable>();

    void Awake()
    {
        foreach(Spawnable thingToSpawn in spawn)
        {
            PhotonNetwork.Instantiate(thingToSpawn.name, thingToSpawn.position, Quaternion.identity);
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
