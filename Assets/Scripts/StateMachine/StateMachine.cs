public class StateMachine
{
    IState currentState;

    public void Update()
    {
        currentState?.OnStateUpdate();
    }

    public void ChangeState(IState newState)
    {
        currentState?.OnStateExit();
        currentState = newState;
        currentState?.OnStateEnter();
    }
}