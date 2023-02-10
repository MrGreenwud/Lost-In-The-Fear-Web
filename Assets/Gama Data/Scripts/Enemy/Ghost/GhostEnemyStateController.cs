public class GhostEnemyStateController : BaseEnemyStateController
{
    new protected GhostEnemyController p_Controller;

    new public GhostEnemySearch EnemySearch { get; private set; }

    public EnemyTeleport EnemyTeleport { get; private set; }

    public GhostEnemyStateController(GhostEnemyController controller) : base(controller)
    {
        p_Controller = controller;
        InitStates();
        SwichState(EnemySearch);
    }

    protected override void InitStates()
    {
        EnemySearch = new GhostEnemySearch(p_Controller, this);
        EnemySearch.Init(EnemySearch);

        EnemyWalk = new EnemyWalk(p_Controller, this);
        EnemyWalk.Init(EnemyWalk);

        EnemyRun = new EnemyRun(p_Controller, this);
        EnemyRun.Init(EnemyRun);

        EnemyTeleport = new EnemyTeleport(p_Controller, this);
        EnemyTeleport.Init(EnemyTeleport);

        EnemyWalk.AddTransition(EnemySearch);
        EnemyWalk.AddTransition(EnemyRun);
        EnemyWalk.AddTransition(EnemyTeleport);

        EnemyRun.AddTransition(EnemySearch);
        EnemyRun.AddTransition(EnemyWalk);
        EnemyRun.AddTransition(EnemyTeleport);

        EnemySearch.AddTransition(EnemyWalk);
        EnemySearch.AddTransition(EnemyRun);
        EnemySearch.AddTransition(EnemyTeleport);

        EnemyTeleport.AddTransition(EnemyRun);
        EnemyTeleport.AddTransition(EnemyWalk);
        EnemyTeleport.AddTransition(EnemySearch);
    }
}