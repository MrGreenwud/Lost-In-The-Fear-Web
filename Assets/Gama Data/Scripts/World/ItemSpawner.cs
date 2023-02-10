using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Zenject;
using NaughtyAttributes;

public class ItemSpawner : MonoBehaviour
{
    [Serializable]
    public class SpawnSettings 
    {
        [SerializeField] private GameObject m_ItemPrefab;
        [SerializeField] [Range(0, 1)] private float m_Weight;

        public GameObject GetItemPrefab() => m_ItemPrefab;
        public float GetWeight() => m_Weight;
    }

    [Inject] private readonly PlayerController m_PlayerController;

    [SerializeField] private SpawnSettings[] m_SpawnSettings;
    [SerializeField] private int m_ItemCount = 1;

    [Space(10)]

    [SerializeField] private float m_CheckSphereRadius = 5;
    [SerializeField] private LayerMask m_SpawnPointLayer;

    [Space(10)]

    [SerializeField] private List<ItemSpawnPoint> m_SpawnPositions = new List<ItemSpawnPoint>();
    [SerializeField] private float m_RespawnTime = 60;

    [Space(10)]

    [SerializeField] private AudioSource m_AudioSource;

    private void Awake()
    {
        Queue<ItemSpawnPoint> points = new Queue<ItemSpawnPoint>();
        List<ItemSpawnPoint> tempPoints = GetItemRandomPositions(m_SpawnPositions.Count / 5, m_SpawnPositions.Count / 2);

        for (int i = 0; i < tempPoints.Count; i++)
        {
            if (tempPoints[i] == null) continue;
            points.Enqueue(m_SpawnPositions[i]);
        }

        foreach (ItemSpawnPoint itemSpawnPoint in points)
        {
            if (itemSpawnPoint.IsFull == true) continue;
            Spawn(itemSpawnPoint);
        }
    }

    public void OnRespawn()
    {
        StartCoroutine(Respawn());
    }


    public void PutSpwanPoint(ItemSpawnPoint newItemSpwanPoint)
    {
        if (newItemSpwanPoint == null) return;
        m_SpawnPositions.Add(newItemSpwanPoint);
    }

    private void Spawn(ItemSpawnPoint itemSpawnPoint)
    {
        while(true)
        {
            if (itemSpawnPoint.IsFull == true) return;

            int randomItem = Random.Range(0, m_SpawnSettings.Length);
            float randomWeight = Random.Range(0, 100);

            //Debug.Log($"Point: {itemSpawnPoint} Item index: {randomItem} Random Weight: {randomWeight}");

            if (m_SpawnSettings[randomItem].GetWeight() * 100 >= randomWeight)
            {
                Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                
                GameObject item = Instantiate(m_SpawnSettings[randomItem].GetItemPrefab(), itemSpawnPoint.transform.position, rotation);
                itemSpawnPoint.IsFull = true;

                if (item.TryGetComponent<GameObjectItem>(out GameObjectItem gameObjectItem))
                {
                    gameObjectItem.SetItemSpawnPoint(itemSpawnPoint);

                    if(m_AudioSource != null)
                        gameObjectItem.SetAudioSource(m_AudioSource);
                }
            }
        }
    }

    private void AllItemsSpwan()
    {
        Queue<ItemSpawnPoint> points = new Queue<ItemSpawnPoint>();

        for (int i = 0; i < m_SpawnPositions.Count; i++)
        {
            if (m_SpawnPositions[i] == null) continue;
            points.Enqueue(m_SpawnPositions[i]);
        }

        foreach (ItemSpawnPoint itemSpawnPoint in points)
        {
            if (itemSpawnPoint.IsFull == true) continue;
            Spawn(itemSpawnPoint);
        }
    }

    private List<ItemSpawnPoint> GetItemRandomPositions(int minCount, int maxCount)
    {
        List<ItemSpawnPoint> tempPoints = new List<ItemSpawnPoint>();

        for(int i = 0; i < m_SpawnPositions.Count; i++)
        {
            if (m_SpawnPositions[i] == null) continue;
            tempPoints.Add(m_SpawnPositions[i]);
        }

        if (maxCount > tempPoints.Count)
            maxCount = tempPoints.Count;

        if(minCount < 0)
            minCount = 0;

        List<ItemSpawnPoint> newPoints = new List<ItemSpawnPoint>();

        int count = Random.Range(minCount, maxCount);
        
        for (int i = 0; i < count; i++)
        {
            if (tempPoints.Count <= 0) break;

            int index = Random.Range(0, tempPoints.Count);
            newPoints.Add(tempPoints[index]);
            tempPoints.Remove(tempPoints[index]);
        }

        return newPoints;
    }

    private Queue<ItemSpawnPoint> GetFarItemSpawnPoints(List<ItemSpawnPoint> activePoints)
    {
        Vector3 checkSpherePosition = m_PlayerController.transform.position;

        RaycastHit[] hits = Physics.SphereCastAll(checkSpherePosition, m_CheckSphereRadius, Vector3.up * 0.1f, m_SpawnPointLayer);
        Queue<ItemSpawnPoint> itemSpwanPointsNearPlayer = new Queue<ItemSpawnPoint>();

        for(int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.TryGetComponent<ItemSpawnPoint>(out ItemSpawnPoint itemSpwanPoint))
                itemSpwanPointsNearPlayer.Enqueue(itemSpwanPoint);
        }

        Queue<ItemSpawnPoint> spawnPoints = new Queue<ItemSpawnPoint>();

        for(int i = 0; i < activePoints.Count; i++)
        {
            if (itemSpwanPointsNearPlayer.Contains(activePoints[i]) == false)
                spawnPoints.Enqueue(activePoints[i]);
        }

        return spawnPoints;
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSecondsRealtime(m_RespawnTime);

        Debug.Log("Respawn Items");

        Queue<ItemSpawnPoint> itemSpawnPoints = GetFarItemSpawnPoints(GetItemRandomPositions(m_SpawnPositions.Count / 5, m_SpawnPositions.Count / 3));

        foreach (ItemSpawnPoint itemSpawnPoint in itemSpawnPoints)
        {
            if (itemSpawnPoint.IsFull == true) continue;
            Spawn(itemSpawnPoint);
        }

        StartCoroutine(Respawn());
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < m_SpawnPositions.Count; i++)
        {
            Gizmos.color = Color.white;
            if (m_SpawnPositions[i] == null)
            {
                continue;
                Debug.LogError(i);
            }

            Gizmos.DrawCube(m_SpawnPositions[i].transform.position + new Vector3(0, 1, 0), Vector3.one * 0.2f);
        }
    }
}
