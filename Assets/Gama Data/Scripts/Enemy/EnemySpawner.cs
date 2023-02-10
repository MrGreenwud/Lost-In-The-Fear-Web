using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static EnemySpawner.EnemyInfo;

public class EnemySpawner : MonoBehaviour
{
    [Serializable]
    public struct EnemyInfo
    {
        public enum EnemyType
        {
            Ghost
        }

        [SerializeField] private EnemyType m_Type;
        [SerializeField] private GameObject m_Prifab;
        [SerializeField] private int m_Count;

        public EnemyType GetEnemyType() => m_Type;
        public GameObject GetPrefab() => m_Prifab;
        public int GetCount() => m_Count;
    }

    [Inject] private readonly PlayerController m_PlayerController;
    
    public Action OnSpawn;
    public Action OnDestroy;

    [SerializeField] private List<EnemyController> m_EnemyPool;

    [SerializeField] private EnemyInfo[] EnemyInfos;
    [SerializeField] private List<GameObject> m_Contaners;

    [SerializeField] private bool m_IsInitPool = true;

    protected Vector3 p_SpawnPosition;
    protected Quaternion p_SpawnRotation;

    private Transform m_Target;

    public void SetSpawnPosition(Vector3 position) => p_SpawnPosition = position;
    public void SetSpwanRotation(Quaternion rotation) => p_SpawnRotation = rotation;

    private void Awake()
    {
        if(m_IsInitPool == true)
            InitPool();
    }

    protected virtual void InitPool()
    {
        m_Target = m_PlayerController.transform;

        int enemyInfoUnitCount = Enum.GetNames(typeof(EnemyType)).Length;

        for (int i = 0; i < enemyInfoUnitCount; i++)
        {
            GameObject contaner = new GameObject(Enum.GetName(typeof(EnemyType), i));
            m_Contaners.Add(contaner);
        }

        for(int i = 0; i < EnemyInfos.Length; i++)
        {
            Transform contaner = m_Contaners[i].transform;

            for (int a = 1; a <= EnemyInfos[i].GetCount(); a++)
                InstantiateEnemy(EnemyInfos[i], contaner);
        }
    }

    public virtual void InstantiateEnemy(EnemyInfo enemyInfo, Transform contaner)
    {
        GameObject enemy = Instantiate(enemyInfo.GetPrefab(), contaner);
        enemy.SetActive(false);

        if (enemy.TryGetComponent<EnemyController>(out EnemyController enemyController))
        {
            enemyController.Init(m_PlayerController, this);
            m_EnemyPool.Add(enemyController);
        }
        else
        {
            Debug.LogError("Spwan object is not Enemy!");
            Destroy(enemy);
            return;
        }
    }

    public virtual void Spwan(EnemyInfo enemyInfo, int count = 1, bool createNewEnemys = false)
    {
        if(enemyInfo.GetCount() < count)
        {
            if(createNewEnemys == true)
            {
                Debug.LogWarning("Instantiate new Enemy!");
                
                for (int i = 0; i < m_Contaners.Count; i++)
                {
                    if (m_Contaners[i].name == Enum.GetName(typeof(EnemyType), enemyInfo.GetEnemyType()))
                    {
                        for (int a = 0; a < count - enemyInfo.GetCount(); a++)
                            InstantiateEnemy(enemyInfo, m_Contaners[i].transform);
                    }
                }
            }
            else
            {
                Debug.LogError("Not enough Enemies in Pool!");
                return;
            }
        }

        List<EnemyController> enemysRemoved = new List<EnemyController>();

        for(int i = 0; i < count; i++)
        {
            if (m_EnemyPool[i].Type == enemyInfo.GetEnemyType())
            {
                OnSpawn?.Invoke();

                m_EnemyPool[i].gameObject.transform.SetPositionAndRotation(p_SpawnPosition, p_SpawnRotation);
                m_EnemyPool[i].gameObject.SetActive(true);
                enemysRemoved.Add(m_EnemyPool[i]);
            }
        }

        for(int i = 0; i < enemysRemoved.Count; i++)
            m_EnemyPool.Remove(enemysRemoved[i]);
    }

    public virtual void SpwanWhithType(EnemyType enemyType, int count = 1, bool createNewEnemy = false)
    {
        for(int i = 0; i < EnemyInfos.Length; i++)
        {
            if (EnemyInfos[i].GetEnemyType() == enemyType)
            {
                Spwan(EnemyInfos[i], count, createNewEnemy);
                return;
            }
        }

        Debug.LogError("No suitable EnemyInfo for this enemy Type!");
    }

    public virtual void DestroyEnemy(EnemyController enemy)
    {
        OnDestroy?.Invoke();
        m_EnemyPool.Add(enemy);
        enemy.gameObject.SetActive(false);
    }
}