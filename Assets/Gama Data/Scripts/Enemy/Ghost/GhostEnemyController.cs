using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GhostEnemyController : BaseEnemyController
{
    new public GhostEnemyStateController StateController { get; private set; }

    public EnemyTeleporter EnemyTeleporter { get; protected set; }

    [Header("Attack Settings")]

    [SerializeField] private float m_AttackDistence = 2;
    [SerializeField] private float m_AttackStanTime = 10;
    [SerializeField] private LayerMask m_AttackObstacleLayer;

    [Header("Telepot Settings")]

    [SerializeField] private float m_TeleportCoolDown = 20;
    [SerializeField] private float m_TeleportDistence = 10;
    [SerializeField] private LayerMask m_TeleportPointLayer;

    [Space(10)]

    [SerializeField] private float m_TeleportForwordCeterOffset;
    [SerializeField] private float m_TeleportBackCeterOffset;

    [SerializeField] private FoodSteps m_FoodSteps;

    public float GetTeleportDistence() => m_TeleportDistence;
    public float GetTeleportCoolDown() => m_TeleportCoolDown;

    public LayerMask GetTeleportPointLayer() => m_TeleportPointLayer;

    public float GetTeleportForwordCeterOffset() => m_TeleportForwordCeterOffset;
    public float GetTeleportBackCeterOffset() => m_TeleportBackCeterOffset;

    private void OnEnable()
    {
        m_IsStanAudioClip = true;
    }

    public override void Init(PlayerController playerController, EnemySpawner enemySpawner)
    {
        base.Init(playerController, enemySpawner);

        EnemyTeleporter = new EnemyTeleporter(this);

        StateController = new GhostEnemyStateController(this);
        base.StateController = StateController;
    }

    protected override void Tick()
    {
        base.Tick();

        EnemyTeleporter.Update();

        if (IsFollow == true)
            NavMeshAgent.stoppingDistance = 2;
        else
            NavMeshAgent.stoppingDistance = 0;

        float distence = Vector3.Distance(transform.position, Target.position);

        if (distence < m_AttackDistence)
        {
            Vector3 direction = (Target.position - transform.position).normalized;

            if (!Physics.Raycast(transform.position, direction, distence, m_AttackObstacleLayer))
                Attack();
        }

        m_FoodSteps.Step(IsRun);
    }

    [SerializeField] private AudioClip m_StanAudioClip;
    [SerializeField] private AudioSource m_AudioSource;

    private bool m_IsStanAudioClip = true;

    public override void ApplyStan(float time)
    {
        base.ApplyStan(time);
        p_StanTime = time;

        if (m_IsStanAudioClip)
        {
            m_AudioSource.transform.position = transform.position;
            m_AudioSource.PlayOneShot(m_StanAudioClip);
        }

        p_EnemySpawner.DestroyEnemy(this);
    }

    public override void Attack()
    {
        m_IsStanAudioClip = false;
        p_PlayerController.PlayerStats.ApplyDamage(1);
        ApplyStan(m_AttackStanTime);
        ItemUser.Instance.Protactor.OnEnemyStan?.Invoke(this, m_AttackStanTime);
        base.Attack();
    }

#if UNITY_EDITOR

    protected override void GizmosUpdate()
    {
        base.GizmosUpdate();

        if (Application.IsPlaying(gameObject) == true)
            EnemyTeleporter.GizmosUpdate();
    }

#endif
}