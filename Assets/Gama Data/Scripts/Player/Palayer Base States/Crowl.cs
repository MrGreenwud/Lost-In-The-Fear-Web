public class Crowl : PlayerState
{
    public Crowl(PlayerStateController playerStateController, PlayerController playerController) : base(playerStateController, playerController) { }

    public override void Enter()
    {
        base.Enter();
        p_PlayerController.PlayerCroucher.Crouch();
    }

    public override void Update()
    {
        p_PlayerController.PlayerMover.Move(p_PlayerController.GetCrowlSpeed());

        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
        p_PlayerController.PlayerCroucher.Stay();
    }

    public override void TransitionLogic()
    {
        base.TransitionLogic();

        if (p_PlayerController.PlayerGravity.ChackGround() == true)
        {
            if (p_PlayerController.IsMove == true)
            {
                if (p_PlayerController.IsCrouch == true)
                {
                    if (p_PlayerController.IsJump == false)
                        p_PlayerStateController.SwichState(p_ThisState);
                }
            }
        }
    }
}
