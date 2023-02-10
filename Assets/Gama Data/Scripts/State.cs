using System.Collections.Generic;

public class State
{
    private List<State> m_States = new List<State>();
    protected State p_ThisState;

    public virtual void Init(State thisState)
    {
        p_ThisState = thisState;
    }

    public virtual void Enter() { }

    public virtual void Update()
    {
        if (m_States == null) return;

        for (int i = 0; i < m_States.Count; i++)
            m_States[i].TransitionLogic();
    }

    public virtual void PhisicsUpdate() { }

    public virtual void Exit() { }

    public virtual void TransitionLogic() { }

    public void AddTransition(State newState)
    {
        if (newState == null) return;
        m_States.Add(newState);
    }
}