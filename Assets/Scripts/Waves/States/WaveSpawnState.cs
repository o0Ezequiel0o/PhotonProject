using UnityEngine;

public class WaveSpawnState : IState
{
    readonly WaveManager waveManager;

    float timer = 0f;

    public WaveSpawnState(WaveManager waveManager)
    {
        this.waveManager = waveManager;
    }

    public void OnStateEnter()
    {
        timer = 0f;
    }

    public void OnStateUpdate()
    {
        if (waveManager.UnactiveSpawnables == 0)
        {
            waveManager.stateMachine.ChangeState(waveManager.waveWaitState);
        }

        UpdateSpawnTimer();
    }

    void UpdateSpawnTimer()
    {
        timer += Time.deltaTime;

        if (timer >= waveManager.LoadedWaveUpdate.SpawnCooldown)
        {
            waveManager.ActivateNextUnactiveInstantiatedSpawnable();
            timer = 0f;
        }
    }

    public void OnStateExit() {}
}