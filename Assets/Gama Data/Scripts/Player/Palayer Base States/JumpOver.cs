using UnityEngine;

public class JumpOver : PlayerState
{
    public JumpOver(PlayerStateController playerStateController, PlayerController playerController) : base(playerStateController, playerController) { }

    public override void Update()
    {
        p_PlayerController.PlayerJamper.Jump();

        base.Update();
    }

    public override void TransitionLogic()
    {
        base.TransitionLogic();

        if(p_PlayerController.IsJump == true)
        {
            p_PlayerStateController.SwichState(p_ThisState);
        }
    }
}
