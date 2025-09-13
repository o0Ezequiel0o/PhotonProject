public class WaveWaitState : IState
{
    readonly WaveManager waveManager;

    public WaveWaitState(WaveManager waveManager)
    {
        this.waveManager = waveManager;
    }

    public void OnStateEnter() {}

    public void OnStateExit() {}

    public void OnStateUpdate()
    {
        if (waveManager.ActiveSpawnables == 0)
        {
            waveManager.stateMachine.ChangeState(waveManager.waveLoadState);
        }
    }
}