using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Runtime.InteropServices;

[ExecuteAlways]
public class SpawnPointCreator : MonoBehaviour
{
    [Header("Spwan Point Creator")]

    [Space(5)]

    [SerializeField] private GameObject m_SpawnPointPrefab;
    [SerializeField] private uint m_FrameSimulateCount;

    [SerializeField] [Range(0, 50)] private int m_SpawnPointCount;

    [SerializeField] private float m_Radius = 3f;
    [SerializeField] private float m_Force = 2500f;

    [SerializeField] private bool m_UseZone = true;

    [ShowIf("m_UseZone")]
    [SerializeField] private Vector3 m_BoxSize = new Vector3(5, 2, 6);

    [SerializeField] private List<Vector3> m_Positions = new List<Vector3>();
    [SerializeField] private List<GameObject> m_SpawnPoints = new List<GameObject>();
    [SerializeField] private ItemSpawner m_ItemSpawner;

    private List<GameObject> m_SimulationCubes = new List<GameObject>();
    
    private uint m_FrameCount;
    private bool m_PhysicsEnabled;
    
    [Button("Auto")]
    private void Auto()
    {
        Create();
        Simulate();
    }

    [Button("Create")]
    private void Create()
    {
        Remove();

        for (int i = 0; i < m_SpawnPointCount; i++)
        {
            float xPosition = Random.Range(transform.position.x - m_Radius, transform.position.x + m_Radius);
            float yPosition = Random.Range(transform.position.y - m_Radius, transform.position.y + m_Radius);
            float zPosition = Random.Range(transform.position.z - m_Radius, transform.position.z + m_Radius);

            float xRotatin = Random.Range(0, 360);
            float yRotatin = Random.Range(0, 360);
            float zRotatin = Random.Range(0, 360);

            Vector3 postion = new Vector3(xPosition, yPosition, zPosition);
            Quaternion rotatin = Quaternion.Euler(xRotatin, yRotatin, zRotatin);

            GameObject SpawnObject = Instantiate(m_SpawnPointPrefab, postion, rotatin, transform);

            m_SimulationCubes.Add(SpawnObject);
        }
    }

    [Button("Remove")]
    private void Remove()
    {
        for(int i = 0; i < m_SimulationCubes.Count; i++)
        {
            if (m_SimulationCubes[i] == null) continue;
            DestroyImmediate(m_SimulationCubes[i]);
        }

        m_SimulationCubes.Clear();
    }

    [Button("Clear All Positions")]
    private void ClearAllPoistions()
    {
        m_Positions.Clear();
    }

    [Button("Simulate")]
    private void Simulate()
    {
        if (m_SimulationCubes.Count == 0) return;

        m_FrameCount = 0;

        for (int i = 0; i < m_SimulationCubes.Count; i++)
        {
            Rigidbody rigidbody = m_SimulationCubes[i].GetComponent<Rigidbody>();
            rigidbody.AddExplosionForce(m_Force, transform.position, m_Radius);
        }

        m_PhysicsEnabled = true;
    }

    [Button("Save Positions")]
    private void SavePositions()
    {
        for(int i = 0; i < m_SimulationCubes.Count; i++)
        {
            if (m_SimulationCubes[i] == null) continue;

            Vector3 position = m_SimulationCubes[i].transform.position;

            if (m_UseZone == true)
            {
                if (position.x < transform.position.x + m_BoxSize.x / 2 && position.x > transform.position.x - m_BoxSize.x / 2)
                {
                    if (position.y < transform.position.y + m_BoxSize.y / 2 && position.y > transform.position.y - m_BoxSize.y / 2)
                    {
                        if (position.z < transform.position.z + m_BoxSize.z / 2 && position.z > transform.position.z - m_BoxSize.z / 2)
                        {
                            RaycastHit hit;
                            if (Physics.Raycast(m_SimulationCubes[i].transform.position, Vector3.down, out hit, 100))
                                m_Positions.Add(hit.point);
                        }
                    }
                }
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(m_SimulationCubes[i].transform.position, Vector3.down, out hit, 100))
                    m_Positions.Add(hit.point);
            }
        }
    }

    [Button("Create Spawn Points")]
    private void CreateSpawnPoints()
    {
        for (int i = 0; i < m_Positions.Count; i++)
        {
            if (m_Positions[i] == null) continue;

            GameObject point = new GameObject("Onject spawn Point");
            
            point.transform.position = m_Positions[i];
            point.transform.rotation = Quaternion.identity;
            
            point.AddComponent<ItemSpawnPoint>();
            point.GetComponent<ItemSpawnPoint>().SetIsTrigger();

            m_SpawnPoints.Add(point);
        }
    }

    [Button("Put Spwan Points")]
    private void PutSpwanPoints()
    {
        if (m_ItemSpawner == null) return;

        for (int i = 0; i < m_SpawnPoints.Count; i++)
        {
            if (m_SpawnPoints[i] == null) continue;

            if (m_SpawnPoints[i].TryGetComponent<ItemSpawnPoint>(out ItemSpawnPoint itemSpawnPoint))
                m_ItemSpawner.PutSpwanPoint(itemSpawnPoint);
        }

        m_SpawnPoints.Clear();
    }

    private void Update()
    {
        if (m_PhysicsEnabled == true)
        {
            if (m_FrameCount < m_FrameSimulateCount)
            {
                Physics.autoSimulation = false;
                Physics.Simulate(Time.fixedDeltaTime);
                Physics.autoSimulation = true;

                m_FrameCount++;
            }
            else
            {
                m_PhysicsEnabled = false;
            }
        }
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (m_FrameCount < m_FrameSimulateCount)
        {
            if (!Application.isPlaying)
            {
                UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
                UnityEditor.SceneView.RepaintAll();
            }
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_Radius);

        if (m_UseZone == true)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, m_BoxSize);
        }

        Gizmos.color = Color.magenta;

        for (int i = 0; i < m_SpawnPoints.Count; i++)
        {
            if (m_SpawnPoints[i] == null) continue;
            Gizmos.DrawCube(m_SpawnPoints[i].transform.position + new Vector3(0, 1, 0), Vector3.one * 0.2f);
        }
    }

#endif
}
