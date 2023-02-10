using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Way : MonoBehaviour
{
    [SerializeField] private List<Transform> m_Points;
    [SerializeField] private Color m_WayColor;

    [SerializeField] private bool m_IsReturn;

    public Vector3[] GetPoints()
    {
        Vector3[] positions = new Vector3[m_Points.Count];

        for (int i = 0; i < m_Points.Count; i++)
        {
            if (m_Points[i] == null)
            {
                Debug.LogError($"Point with index: {i} in Way: {name} is null");
                continue;
            }

            positions[i] = m_Points[i].position;
        }

        return positions;
    }

    public Color GetWayColor() => m_WayColor;
    public bool GetIsReturn() => m_IsReturn;

    [Button("Create new point")]
    private void CreatePoint()
    {
        Vector3 createPosition = transform.position;
        Vector3 direction = transform.forward;
        const float offset = 5;
        
        if (m_Points.Count >= 2)
            direction = (m_Points[m_Points.Count - 1].position - m_Points[m_Points.Count - 2].position).normalized;

        if (m_Points.Count > 0)
            createPosition = m_Points[m_Points.Count - 1].position + direction * offset;

        GameObject newPoint = new GameObject();
        newPoint.name = $"New Player Way Point {m_Points.Count}";
        newPoint.transform.position = createPosition;
        newPoint.transform.parent = transform;

        m_Points.Add(newPoint.transform);
    }

    [Button("Clear Points")]
    private void ClearPoints()
    {
        for(int i = 0; i < m_Points.Count; i++)
            DestroyImmediate(m_Points[i].gameObject);

        m_Points.Clear();
    }
}
