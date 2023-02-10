public class PlayerState : State
{
    protected PlayerController p_PlayerController;
    protected PlayerStateController p_PlayerStateController;
    protected PlayerState p_ThisState;

    public PlayerState(PlayerStateController playerStateController, PlayerController playerController)
    {
        p_PlayerStateController = playerStateController;
        p_PlayerController = playerController;
    }

    public virtual void Init(PlayerState thisState) 
    {
        p_ThisState = thisState;
    }
}
