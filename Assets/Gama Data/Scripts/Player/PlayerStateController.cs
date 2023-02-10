using UnityEngine;

public class PlayerStateController : StateMachine
{
    private PlayerController m_PlayerController;

    public Idel Idel { get; private set; }
    public Walk Walk { get; private set; }
    public Run Run { get; private set; }
    public Crouch Crouch { get; private set; }
    public Crowl Crowl { get; private set; }
    public JumpOver JumpOver { get; private set; }

    public PlayerStateController(PlayerController playerController)
    {
        m_PlayerController = playerController;

        InitStates();
    }

    private void InitStates()
    {
        Idel = new Idel(this, m_PlayerController);
        Idel.Init(Idel);

        Walk = new Walk(this, m_PlayerController);
        Walk.Init(Walk);

        Run = new Run(this, m_PlayerController);
        Run.Init(Run);

        Crouch = new Crouch(this, m_PlayerController);
        Crouch.Init(Crouch);

        Crowl = new Crowl(this, m_PlayerController);
        Crowl.Init(Crowl);

        JumpOver = new JumpOver(this, m_PlayerController);
        JumpOver.Init(JumpOver);

        Idel.AddTransition(Walk);
        Idel.AddTransition(Run);
        Idel.AddTransition(Crouch);
        Idel.AddTransition(Crowl);
        Idel.AddTransition(JumpOver);

        Walk.AddTransition(Idel);
        Walk.AddTransition(Run);
        Walk.AddTransition(Crouch);
        Walk.AddTransition(Crowl);
        Walk.AddTransition(JumpOver);

        Run.AddTransition(Idel);
        Run.AddTransition(Walk);
        Run.AddTransition(Crouch);
        Run.AddTransition(Crowl);
        Run.AddTransition(JumpOver);

        Crouch.AddTransition(Idel);
        Crouch.AddTransition(Walk);
        Crouch.AddTransition(Run);
        Crouch.AddTransition(Crowl);
        Crouch.AddTransition(JumpOver);

        Crowl.AddTransition(Idel);
        Crowl.AddTransition(Walk);
        Crowl.AddTransition(Run);
        Crowl.AddTransition(Crouch);
        Crowl.AddTransition(JumpOver);

        JumpOver.AddTransition(Idel);
        JumpOver.AddTransition(Walk);
        JumpOver.AddTransition(Run);
        JumpOver.AddTransition(Crouch);
        JumpOver.AddTransition(Crowl);

        SwichState(Idel);
    }

    public void Update()
    {
        CurrentState.Update();
    }
}