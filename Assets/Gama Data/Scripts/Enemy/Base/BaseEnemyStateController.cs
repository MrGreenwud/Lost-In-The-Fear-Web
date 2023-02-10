public class BaseEnemyStateController : StateController
{
    protected BaseEnemyController p_Controller;

    public EnemySearch EnemySearch { get; protected set; }
    public EnemyWalk EnemyWalk { get; protected set; }
    public EnemyRun EnemyRun { get; protected set; }

    public BaseEnemyStateController(BaseEnemyController controller)
    {
        p_Controller = controller;
    }

    protected override void InitStates()
    {
        base.InitStates();

        EnemySearch = new EnemySearch(p_Controller, this);
        EnemySearch.Init(EnemySearch);

        EnemyWalk = new EnemyWalk(p_Controller, this);
        EnemyWalk.Init(EnemyWalk);

        EnemyRun = new EnemyRun(p_Controller, this);
        EnemyRun.Init(EnemyRun);

        EnemySearch.AddTransition(EnemyWalk);
        EnemySearch.AddTransition(EnemyRun);

        EnemyWalk.AddTransition(EnemySearch);
        EnemyWalk.AddTransition(EnemyRun);

        EnemyRun.AddTransition(EnemySearch);
        EnemyRun.AddTransition(EnemyWalk);

        SwichState(EnemySearch);
    }
}
