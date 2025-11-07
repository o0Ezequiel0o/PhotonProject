using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private List<ZombieContainer> zombiesToSpawn;
    [SerializeField] private List<Transform> spawnPoints;
    [Space]
    [SerializeField] private float spawnInterval = 5.0f;

    private float timeSinceStart = 0f;
    private float timer = 0f;

    private void Update()
    {
        if (!LocalInstance.isHost) return;

        timeSinceStart += Time.deltaTime;
        timer += Time.deltaTime;

        if (timer > GetSpawnInterval())
        {
            RollZombieType();
            timer = 0f;
        }
    }

    private float GetSpawnInterval()
    {
        return (spawnInterval - 1) * Mathf.Exp(-(0.018f * timeSinceStart)) + 1f;
    }

    private void RollZombieType()
    {
        int maxRollWeight = CalculateMaxRollWeight();
        int weightRoll = UnityEngine.Random.Range(0, maxRollWeight);

        for (int i = 0; i < zombiesToSpawn.Count; i++)
        {
            if (zombiesToSpawn[i].weight > weightRoll)
            {
                PhotonNetwork.Instantiate(zombiesToSpawn[i].resourceName, RandomSpawnPoint(), Quaternion.identity);
                return;
            }
            else
            {
                weightRoll -= zombiesToSpawn[i].weight;
            }
        }
    }

    private int CalculateMaxRollWeight()
    {
        int maxRollWeight = 0;

        foreach (ZombieContainer zombieToSpawn in zombiesToSpawn)
        {
            maxRollWeight += zombieToSpawn.weight;
        }

        return maxRollWeight;
    }

    private Vector3 RandomSpawnPoint()
    {
        return spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)].position;
    }

    [Serializable]
    public struct ZombieContainer
    {
        public string resourceName;
        public int weight;
    }
}