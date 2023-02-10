public class EnemyWalk : BaseEnemyState
{
    public EnemyWalk(BaseEnemyController controller, BaseEnemyStateController stateController) : base(controller, stateController) { }

    public override void Update()
    {
        p_Controller.EnemyMover.Move(p_Controller.Target.position, p_Controller.GetWalkSpeed());

        base.Update();
    }

    public override void TransitionLogic()
    {
        base.TransitionLogic();

        if (p_Controller.IsFollow == true)
        {
            if(p_Controller.IsRun == false)
                p_StateController.SwichState(p_ThisState);
        }
    }
}
