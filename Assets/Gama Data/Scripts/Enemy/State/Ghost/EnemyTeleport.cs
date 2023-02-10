using UnityEngine;

public class EnemyTeleport : GhostEnemyState
{
    public EnemyTeleport(GhostEnemyController controller, GhostEnemyStateController stateController) : base(controller, stateController) { }

    public override void Enter()
    {
        base.Enter();

        p_Controller.NavMeshAgent.isStopped = true;
    }

    public override void Update()
    {
        p_Controller.EnemyTeleporter.Teleport();

        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
        p_Controller.NavMeshAgent.isStopped = false;
    }

    public override void TransitionLogic()
    {
        base.TransitionLogic();

        if(p_Controller.IsFollow == false)
        {
            if (p_Controller.EnemyTeleporter.TeleportTimer <= 0)
            {
                p_StateController.SwichState(p_ThisState);
            }
        }
    }
}
