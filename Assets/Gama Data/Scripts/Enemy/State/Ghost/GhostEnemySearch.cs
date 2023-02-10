public class GhostEnemySearch : GhostEnemyState
{
    public GhostEnemySearch(GhostEnemyController controller, GhostEnemyStateController stateController) : base(controller, stateController) { }

    public override void Update()
    {
        p_Controller.EnemySearcher.Search();

        base.Update();
    }

    public override void TransitionLogic()
    {
        base.TransitionLogic();

        if (p_Controller.IsFollow == false)
        {
            if(p_Controller.EnemyTeleporter.TeleportTimer > 0)
                p_StateController.SwichState(p_ThisState);
        }
    }
}