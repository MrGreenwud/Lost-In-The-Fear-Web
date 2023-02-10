public class Idel : PlayerState
{
    public Idel(PlayerStateController playerStateController, PlayerController playerController) : base(playerStateController, playerController) { }

    public override void Update()
    {
        base.Update();
    }

    public override void TransitionLogic()
    {
        base.TransitionLogic();

        if (p_PlayerController.PlayerGravity.ChackGround() == true)
        {
            if (p_PlayerController.IsCrouch == false)
            {
                if (p_PlayerController.IsMove == false)
                {
                    if (p_PlayerController.IsJump == false)
                        p_PlayerStateController.SwichState(p_ThisState);
                }
            }
        }
    }
}
