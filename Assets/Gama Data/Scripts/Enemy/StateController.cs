public class StateController : StateMachine
{
    protected virtual void InitStates() { }

    public virtual void Update()
    {
        if (CurrentState == null) return;

        CurrentState.Update();
    }
}
