using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Zenject;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(FieldOfView))]
public class EnemyController : MonoBehaviour, IEnemyPooled
{
    [Inject] protected PlayerController p_PlayerController { get; private set; }
    [Inject] protected EnemySpawner p_EnemySpawner { get; private set; }

    public UnityEvent OnAttack;
    public UnityEvent OnApplyStan;
    public UnityEvent<float> OnApplyDamage;

    protected float p_StanTime;

    public NavMeshAgent NavMeshAgent { get; private set; }
    public FieldOfView FieldOfView { get; private set; }
    public Transform Target { get; private set; }

    public bool IsFollow { get; private set; }

    public EnemySpawner.EnemyInfo.EnemyType Type => type;

    [SerializeField] private EnemySpawner.EnemyInfo.EnemyType type;

    private void Start()
    {
        if(p_PlayerController != null)
            Target = p_PlayerController.transform;

        Init(p_PlayerController, p_EnemySpawner);
    }

    public virtual void Init(PlayerController playerController, EnemySpawner enemySpawner)
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        FieldOfView = GetComponent<FieldOfView>();

        if (playerController != null)
        {
            p_PlayerController = playerController;
            Target = playerController.transform;
        }

        if(enemySpawner != null)
            p_EnemySpawner = enemySpawner;
        
        IsFollow = FieldOfView.Check(Target);
    }

    private void Update()
    {
        Tick();
    }

    protected virtual void Tick()
    {
        IsFollow = FieldOfView.Check(Target);
    }

    public virtual void ApplyDamage(float damageValue)
    {
        OnApplyDamage?.Invoke(damageValue);
    }

    public virtual void ApplyStan(float time)
    {
        OnApplyStan?.Invoke();
    }

    public virtual void Attack()
    {
        OnAttack?.Invoke();
    }

    private void OnDrawGizmos()
    {
        GizmosUpdate();
    }

    protected virtual void GizmosUpdate() { }
}
