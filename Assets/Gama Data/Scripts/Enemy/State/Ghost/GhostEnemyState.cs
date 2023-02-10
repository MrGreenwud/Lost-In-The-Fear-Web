public class GhostEnemyState : State
{
    protected GhostEnemyController p_Controller;
    protected GhostEnemyStateController p_StateController;

    public GhostEnemyState(GhostEnemyController controller, GhostEnemyStateController stateController)
    {
        p_Controller = controller;
        p_StateController = stateController;
    }
}
