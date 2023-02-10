using UnityEngine;

public class BaseEnemyController : EnemyController
{
    public BaseEnemyStateController StateController { get; protected set; }
    public EnemyMover EnemyMover { get; protected set; }
    public EnemySearcher EnemySearcher { get; protected set; }

    public bool IsRun { get; private set; }

    [Space(10)]
    [Header("Move")]

    [SerializeField] private float m_WalkSpeed;
    [SerializeField] private float m_RunSpeed;
    [SerializeField] protected LayerMask m_ObstacleLayer;

    [Space(10)]
    [Header("Search")]

    [SerializeField] private float m_SearchDistence = 20;

    public float GetWalkSpeed() => m_WalkSpeed;
    public float GetRunSpeed() => m_RunSpeed;

    public float GetSearchDistence() => m_SearchDistence;

    public override void Init(PlayerController playerController, EnemySpawner enemySpawner)
    {
        base.Init(playerController, enemySpawner);

        StateController = new BaseEnemyStateController(this);
        EnemyMover = new EnemyMover(this);
        EnemySearcher = new EnemySearcher(this);
    }

    protected override void Tick()
    {
        base.Tick();

        IsRun = CheckRun();
        StateController.Update();
    }

    protected virtual bool CheckRun()
    {
        Vector3 diraction = Target.position - transform.position;
        float distance = Vector3.Distance(transform.position, Target.position);

        if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), diraction, distance, m_ObstacleLayer))
            return false;
        else
            return true;
    }

#if UNITY_EDITOR

    protected override void GizmosUpdate()
    {
        base.GizmosUpdate();

        if (Application.IsPlaying(gameObject) == true)
            EnemyMover.OnDrowGizmos();
    }

#endif

}
