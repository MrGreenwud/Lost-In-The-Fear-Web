public class BaseEnemyState : State
{
    protected BaseEnemyController p_Controller;
    protected StateController p_StateController;

    public BaseEnemyState(BaseEnemyController controller, StateController stateController)
    {
        p_Controller = controller;
        p_StateController = stateController;
    }
}
