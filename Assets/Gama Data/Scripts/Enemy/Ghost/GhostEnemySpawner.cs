using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using EnemyType = EnemySpawner.EnemyInfo.EnemyType;

public class GhostEnemySpawner : MonoBehaviour
{
    [Inject] private readonly EnemySpawner m_EnemySpawner;
    [Inject] private readonly PlayerController m_PlayerController;

    [SerializeField] private float m_SpawnTime = 20;
    [SerializeField] private float m_SpawnDistence = 20;
    [SerializeField] private LayerMask m_TeleportPointLayer;

    private Transform m_Target;
    public Coroutine StanCorotine { get; private set; }

    private Vector3 m_TeleportPostion;

    private void Start()
    {
        m_Target = m_PlayerController.transform;
    }

    private void OnEnable()
    {
        ItemUser.Instance.Protactor.OnEnemyStan += SetEnemyStan;
        m_EnemySpawner.OnSpawn += SetSpawnPosition;
    }

    private void OnDisable()
    {
        ItemUser.Instance.Protactor.OnEnemyStan -= SetEnemyStan;
        m_EnemySpawner.OnSpawn -= SetSpawnPosition;
    }

    public void LetEnemy()
    {
        OnStan(m_SpawnTime);
        Debug.Log(StanCorotine);
    }

    public void SetEnemyStan(EnemyController enemyController, float time)
    {
        if (enemyController is GhostEnemyController)
            OnStan(time);
    }

    public void OnStan(float time)
    {
        StopStan();

        StanCorotine = StartCoroutine(Stan(time));
    }

    public void StopStan()
    {
        if (StanCorotine == null) return;

        StopCoroutine(StanCorotine);
        StanCorotine = null;
    }

    private void SetSpawnPosition()
    {
        RaycastHit[] points = Physics.SphereCastAll(m_Target.position + m_Target.forward * 2, m_SpawnDistence,
            Vector3.up * 0.1f, m_TeleportPointLayer);

        List<Transform> teleportPoints = new List<Transform>();

        for (int i = 0; i < points.Length; i++)
        {
            if (points[i].collider.TryGetComponent<TeleportPoint>(out TeleportPoint teleportPoint))
                teleportPoints.Add(points[i].transform);
        }

        if (teleportPoints.Count > 0)
        {
            Vector3 farPoint = teleportPoints[0].position;
            float distenceToFarPoint = Vector3.Distance(farPoint, m_Target.position);

            for (int i = 1; i < teleportPoints.Count; i++)
            {
                float distenceToCheckPoint = Vector3.Distance(teleportPoints[i].position, m_Target.position);

                if (distenceToFarPoint < distenceToCheckPoint)
                {
                    farPoint = teleportPoints[i].position;
                    distenceToFarPoint = distenceToCheckPoint;
                }
            }

            m_TeleportPostion = farPoint;
        }
        else
        {
            Debug.LogError("Don't find teleport points!");
            m_TeleportPostion = Vector3.zero;
        }

        m_EnemySpawner.SetSpawnPosition(m_TeleportPostion);
    }

    public IEnumerator Stan(float time)
    {
        yield return new WaitForSeconds(time);
        m_EnemySpawner.SpwanWhithType(EnemyType.Ghost, 1, false);
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (Application.IsPlaying(gameObject))
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(m_Target.position + m_Target.forward * (m_SpawnDistence / 3), m_SpawnDistence);
        }
    }

#endif

}