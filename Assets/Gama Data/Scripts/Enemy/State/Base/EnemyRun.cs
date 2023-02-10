public class EnemyRun : BaseEnemyState
{
    public EnemyRun(BaseEnemyController enemyController, BaseEnemyStateController enemyStateController) : base(enemyController, enemyStateController) { }

    public override void Update()
    {
        p_Controller.EnemyMover.Move(p_Controller.Target.position, p_Controller.GetRunSpeed());

        base.Update();
    }

    public override void TransitionLogic()
    {
        base.TransitionLogic();

        if (p_Controller.IsFollow == true)
        {
            if (p_Controller.IsRun == true)
                p_StateController.SwichState(p_ThisState);
        }
    }
}