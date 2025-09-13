using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WaveManager : MonoBehaviour
{
    [field: Header("Settings")]
    [field: SerializeField] public float IntermissionLength { get; private set; }
    [field: SerializeField] private int startPoints;
    
    [field: Header("Spawning")]
    [field: SerializeField] public List<Spawnpoint> Spawnpoints { get; private set; }
    [field: SerializeField] public List<WaveUpdate> WaveUpdates { get; private set; }

    public WaveUpdate LoadedWaveUpdate { get; private set; }

    public int ActiveSpawnables { get; private set; } = 0;
    public int SpawnablesAlive { get; private set; } = 0;

    public int CurrentWave { get; private set; } = 0;
    public int MaxPoints { get; private set; } = 0;

    public readonly StateMachine stateMachine = new StateMachine();

    public WaveSpawnState waveSpawningState;
    public WaveLoadState waveLoadState;
    public WaveWaitState waveWaitState;

    public int UnactiveSpawnables => unactiveInstantiatedSpawnables.Count;
    public int SpawnablesLeftToSpawn => spawnableSpawnRequests.Count;

    private readonly Stack<SpawnableSpawnRequest> spawnableSpawnRequests = new Stack<SpawnableSpawnRequest>();
    private readonly Stack<GameObject> unactiveInstantiatedSpawnables = new Stack<GameObject>();

    private readonly List<FilteredSpawnableData> filteredSpawnablePool = new List<FilteredSpawnableData>();
    private readonly List<WaveSpawnable> currentSpawnablePool = new List<WaveSpawnable>();

    private int maxRollWeight;
    private int points;

    public void GenerateWave()
    {
        while (filteredSpawnablePool.Count > 0)
        {
            FilteredSpawnableData waveSpawnable = RollSpawnable();
            Spawnpoint spawnpoint = waveSpawnable?.GetRandomSpawnpoint();

            if (waveSpawnable != null)
            {
                spawnableSpawnRequests.Push(new SpawnableSpawnRequest(waveSpawnable.prefab, spawnpoint));
                points -= waveSpawnable.cost;
            }
        }
    }

    public void InstantiateUnactiveNextSpawnableInQueue()
    {
        if (spawnableSpawnRequests.Count == 0) return;

        SpawnableSpawnRequest spawnRequest = spawnableSpawnRequests.Pop();
        GameObject spawnableInstance = spawnRequest.Spawn();

        if (spawnableInstance.TryGetComponent(out Damageable damageable))
        {
            damageable.onDeath += ReduceSpawnablesAliveCounter;
        }

        unactiveInstantiatedSpawnables.Push(spawnableInstance);
        spawnableInstance.SetActive(false);
        SpawnablesAlive += 1;
    }

    public void ActivateNextUnactiveInstantiatedSpawnable()
    {
        if (unactiveInstantiatedSpawnables.Count > 0)
        {
            unactiveInstantiatedSpawnables.Pop().SetActive(true);
            ActiveSpawnables += 1;
        }
    }

    public void LoadNextWave()
    {
        CurrentWave += 1;

        if (LoadedWaveUpdate != null)
        {
            MaxPoints += LoadedWaveUpdate.PointsPerWave;
        }

        if (TryGetNextWaveUpdate(out WaveUpdate waveUpdate) && waveUpdate.Wave == CurrentWave)
        {
            LoadNextWaveData();
            UpdateCurrentSpawnablePool();
        }

        GenerateFilteredSpawnablePool();
        RecalculateMaxSpawnRollWeight();

        points = MaxPoints;
    }

    void Awake()
    {
        MaxPoints = startPoints;
        OrderWaveUpdates();
    }

    void Start()
    {
        InitializeStateMachine();
    }

    void Update()
    {
        stateMachine.Update();
    }

    FilteredSpawnableData RollSpawnable()
    {
        int randomWeight = Random.Range(0, maxRollWeight);

        for (int i = 0; i < filteredSpawnablePool.Count; i++)
        {
            if (filteredSpawnablePool[i].weight <= randomWeight)
            {
                randomWeight -= filteredSpawnablePool[i].weight;
                continue;
            }

            if (filteredSpawnablePool[i].cost <= points)
            {
                return filteredSpawnablePool[i];
            }
            else
            {
                filteredSpawnablePool.RemoveAt(i);
                RecalculateMaxSpawnRollWeight();

                return RollSpawnable();
            }
        }

        return null;
    }

    void ReduceSpawnablesAliveCounter()
    {
        SpawnablesAlive -= 1;
        ActiveSpawnables -= 1;
    }

    void InitializeStateMachine()
    {
        waveSpawningState = new WaveSpawnState(this);
        waveLoadState = new WaveLoadState(this);
        waveWaitState = new WaveWaitState(this);

        stateMachine.ChangeState(waveLoadState);
    }

    void OrderWaveUpdates()
    {
        WaveUpdates.OrderByDescending(wave => wave.Wave);
    }

    void GenerateFilteredSpawnablePool()
    {
        filteredSpawnablePool.Clear();

        List<Spawnpoint> possibleSpawnpoints;

        for (int i = 0; i < currentSpawnablePool.Count; i++)
        {
            possibleSpawnpoints = GetPossibleSpawnpoints(currentSpawnablePool[i]);

            if (currentSpawnablePool[i].Cost > MaxPoints)
            {
                continue;
            }

            if (possibleSpawnpoints.Count == 0)
            {
                continue;
            }

            filteredSpawnablePool.Add(new FilteredSpawnableData(currentSpawnablePool[i], possibleSpawnpoints));
        }
    }

    bool TryGetNextWaveUpdate(out WaveUpdate waveUpdate)
    {
        waveUpdate = null;

        if (WaveUpdates.Count != 0 && WaveUpdates[^1] != null)
        {
            waveUpdate = WaveUpdates[^1];
            return true;
        }

        return false;
    }

    void LoadNextWaveData()
    {
        LoadedWaveUpdate = WaveUpdates[^1];
        WaveUpdates.RemoveAt(WaveUpdates.Count - 1);
    }

    void UpdateCurrentSpawnablePool()
    {
        for (int i = 0; i < LoadedWaveUpdate.RemoveFromPool.Count; i++)
        {
            currentSpawnablePool.Remove(LoadedWaveUpdate.RemoveFromPool[i]);
        }

        for (int i = 0; i < LoadedWaveUpdate.AddToPool.Count; i++)
        {
            if (!currentSpawnablePool.Contains(LoadedWaveUpdate.AddToPool[i]))
            {
                currentSpawnablePool.Add(LoadedWaveUpdate.AddToPool[i]);
            }
        }
    }

    void RecalculateMaxSpawnRollWeight()
    {
        maxRollWeight = 0;

        for (int i = 0; i < filteredSpawnablePool.Count; i++)
        {
            maxRollWeight += filteredSpawnablePool[i].weight;
        }
    }

    List<Spawnpoint> GetPossibleSpawnpoints(WaveSpawnable waveSpawnable)
    {
        List<Spawnpoint> possibleSpawnpoints = new List<Spawnpoint>();

        for (int i = 0; i < Spawnpoints.Count; i++)
        {
            if (Spawnpoints[i].Includes(waveSpawnable))
            {
                possibleSpawnpoints.Add(Spawnpoints[i]);
            }
        }

        return possibleSpawnpoints;
    }

    private class FilteredSpawnableData
    {
        public readonly GameObject prefab;
        public readonly List<Spawnpoint> spawnpoints;

        public readonly int weight;
        public readonly int cost;

        private int maxSpawnRollWeight;

        public FilteredSpawnableData(WaveSpawnable waveSpawnable, List<Spawnpoint> spawnpoints)
        {
            prefab = waveSpawnable.Prefab;
            weight = waveSpawnable.Weight;
            cost = waveSpawnable.Cost;

            this.spawnpoints = spawnpoints;
            GetMaxSpawnpointRollWeight();
        }

        public Spawnpoint GetRandomSpawnpoint()
        {
            int randomWeight = Random.Range(0, maxSpawnRollWeight);

            for (int i = 0; i < spawnpoints.Count; i++)
            {
                if (spawnpoints[i].Weight > randomWeight)
                {
                    return spawnpoints[i];
                }
                else
                {
                    randomWeight -= spawnpoints[i].Weight;
                }
            }

            return null;
        }

        void GetMaxSpawnpointRollWeight()
        {
            maxSpawnRollWeight = 0;

            for (int i = 0; i < spawnpoints.Count; i++)
            {
                maxSpawnRollWeight += spawnpoints[i].Weight;
            }
        }
    }

    private readonly struct SpawnableSpawnRequest
    {
        private readonly GameObject prefab;
        private readonly Spawnpoint spawnpoint;

        public SpawnableSpawnRequest(GameObject prefab, Spawnpoint spawnpoint)
        {
            this.prefab = prefab;
            this.spawnpoint = spawnpoint;
        }

        public GameObject Spawn()
        {
            return spawnpoint.Spawn(prefab);
        }
    }
}