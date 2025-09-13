using UnityEngine;

public class WaveLoadState : IState
{
    readonly WaveManager waveManager;

    float intermissionTimer = 0f;

    public WaveLoadState(WaveManager waveManager)
    {
        this.waveManager = waveManager;
    }

    public void OnStateEnter()
    {
        waveManager.LoadNextWave();
        waveManager.GenerateWave();

        intermissionTimer = waveManager.IntermissionLength;
    }

    public void OnStateUpdate()
    {
        intermissionTimer -= Time.deltaTime;

        int spawnablesToPrepareThisFrame = GetSpawnableAmountToPrepareThisFrame();

        for (int i = 0; i < spawnablesToPrepareThisFrame; i++)
        {
            waveManager.InstantiateUnactiveNextSpawnableInQueue();
        }

        if (intermissionTimer <= 0f)
        {
            waveManager.stateMachine.ChangeState(waveManager.waveSpawningState);
        }
    }

    public void OnStateExit() {}

    int GetSpawnableAmountToPrepareThisFrame()
    {
        int framesLeft = Mathf.Max(1, Mathf.CeilToInt(intermissionTimer / Time.deltaTime));

        int averageSpawnablesToPrepare = Mathf.CeilToInt(1 / Time.deltaTime) / 30;
        int minSpawnablesToPrepare = Mathf.CeilToInt((float) waveManager.SpawnablesLeftToSpawn / framesLeft);

        if (averageSpawnablesToPrepare * framesLeft > waveManager.SpawnablesLeftToSpawn)
        {
            return averageSpawnablesToPrepare;
        }
        else
        {
            return minSpawnablesToPrepare;
        }
    }
}