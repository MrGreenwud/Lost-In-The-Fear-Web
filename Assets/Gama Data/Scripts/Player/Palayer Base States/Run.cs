public class Run : PlayerState
{
    public Run(PlayerStateController playerStateController, PlayerController playerController) : base(playerStateController, playerController) { }

    public override void Update()
    {
        p_PlayerController.PlayerMover.Move(p_PlayerController.GetRunSpeed());

        base.Update();
    }

    public override void TransitionLogic()
    {
        base.TransitionLogic();

        if (p_PlayerController.PlayerGravity.ChackGround() == true)
        {
            if (p_PlayerController.IsCrouch == false)
            {
                if (p_PlayerController.IsMove == true)
                {
                    if (p_PlayerController.IsRun == true)
                    {
                        if (p_PlayerController.IsJump == false)
                            p_PlayerStateController.SwichState(p_ThisState);
                    }
                }
            }
        }
    }
}
