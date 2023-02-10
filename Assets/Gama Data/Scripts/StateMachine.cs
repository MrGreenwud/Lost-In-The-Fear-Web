public class StateMachine
{
    public State CurrentState { get; private set; }

    public void SwichState(State newState)
    {
        if (CurrentState == null)
        {
            CurrentState = newState;
            CurrentState.Enter();
        }
        else if (newState != null)
        {
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}
