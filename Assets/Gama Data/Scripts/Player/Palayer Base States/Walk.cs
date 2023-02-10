public class Walk : PlayerState
{
    public Walk(PlayerStateController playerStateController, PlayerController playerController) : base(playerStateController, playerController) { }

    public override void Update()
    {
        p_PlayerController.PlayerMover.Move(p_PlayerController.GetWalkSpeed());

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
                    if (p_PlayerController.IsRun == false)
                    {
                        if (p_PlayerController.IsJump == false)
                            p_PlayerStateController.SwichState(p_ThisState);
                    }
                }
            }
        }
    }
}
