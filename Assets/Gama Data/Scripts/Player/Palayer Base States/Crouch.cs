public class Crouch : PlayerState
{
    public Crouch(PlayerStateController playerStateController, PlayerController playerController) : base(playerStateController, playerController) { }

    public override void Enter()
    {
        base.Enter();
        p_PlayerController.PlayerCroucher.Crouch();
    }

    public override void Exit()
    {
        base.Exit();
        p_PlayerController.PlayerCroucher.Stay();
    }

    public override void TransitionLogic()
    {
        base.TransitionLogic();

        if(p_PlayerController.PlayerGravity.ChackGround() == true)
        {
            if(p_PlayerController.IsMove == false)
            {
                if(p_PlayerController.IsCrouch == true)
                {
                    if (p_PlayerController.IsJump == false)
                        p_PlayerStateController.SwichState(p_ThisState);
                }
            }
        }
    }
}
