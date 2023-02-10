using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;
using NaughtyAttributes;
using UnityEngine.UIElements;

#if UNITY_EDITOR

namespace BCTSTool.World
{
    public enum LineMode
    {
        Points,
        Line
    }

    [Serializable]
    public class Fence
    {
        [ShowAssetPreview]
        [SerializeField] private GameObject m_Prefab;

        [SerializeField][Range(0, 1)] private float m_Weight;

        [SerializeField] private Vector2 m_RotationAngleY;
        [SerializeField] private Vector2 m_RotationAngleZ;

        public GameObject GetPrefab() => m_Prefab;
        public float GetWeight() => m_Weight;

        public Vector2 GetRotationAngleY() => m_RotationAngleY;
        public Vector2 GetRotationAngleZ() => m_RotationAngleZ;
    }

    [ExecuteInEditMode]
    public class FenceRepitor : MonoBehaviour
    {
        [Serializable]
        public class Line
        {
            [SerializeField] private List<Transform> m_Points = new List<Transform>();

            public Line() { }

            public Line(Transform point)
            {
                m_Points.Add(point);
            }

            public Line(List<Transform> points)
            {
                m_Points = points;
            }

            public int GetPointCount() => m_Points.Count;
            public Transform GetLastPoint() => m_Points[m_Points.Count - 1];
            
            public Transform[] GetPoints()
            {
                Transform[] newPointsArray = new Transform[GetPointCount()];

                for (int i = 0; i < m_Points.Count; i++)
                    newPointsArray[i] = m_Points[i];

                return newPointsArray;
            }

            public void AddPoint(Transform newPoint) => m_Points.Add(newPoint);
            public void RemovePoint(Transform point) => m_Points.Remove(point);
            public void RemoveAtPoint(int index) => m_Points.RemoveAt(index);

            public void AddPointByIndex(int index, Transform point)
            {
                if (index > GetPointCount() - 1 || index < 0) return;

                List<Transform> tempPoints = new List<Transform>();

                for (int i = 0; i < m_Points.Count; i++)
                {
                    if (i == index)
                        tempPoints.Add(point);

                    tempPoints.Add(m_Points[i]);
                }

                m_Points = tempPoints;
            }
        }

        [InfoBox("Enebled after apply settings", EInfoBoxType.Warning)]
        public bool m_GenerateInRealTime;
        public LineMode m_LineMode;

        [Space(20)]

        [SerializeField] private GameObject m_PointPrefab;

        [Space(10)]

        [SerializeField] private Transform m_PointsParent;
        [SerializeField] private Transform m_FencesParent;

        [Space(15)]

        [SerializeField] private List<Transform> m_Points;
        [SerializeField] private List<Line> m_Lines = new List<Line>();

        [Space(20)]

        [BoxGroup("Generete Settings")]
        [InfoBox("First argument show min value second max value")]

        [SerializeField] private Fence[] m_Fences;

        [SerializeField] private float m_FenceLength;
        [SerializeField] private LayerMask m_GroundLayer;

        [SerializeField] private List<GameObject> m_FencesObject;
        
        private int m_FanceCount;

        private void Update()
        {
            if (m_LineMode == LineMode.Line && m_Points.Count > 0)
                Debug.LogWarning($"{name} Use Line Mode. Remove Points List for clear memory");
            else if (m_LineMode == LineMode.Points && m_Lines.Count > 0)
                Debug.LogWarning($"{name} Use Poits Mode. Remove Line List for clear memory");

            if (m_GenerateInRealTime == true && Application.IsPlaying(gameObject) == false)
                Generate();
        }

        [Button("Creat point")]
        public void CreatPoint()
        {
            if (m_PointsParent == null)
            {
                GameObject parent = new GameObject($"{name} parent");
                parent.transform.parent = transform;
                m_PointsParent = parent.transform;
            }

            if(m_PointPrefab == null)
            {
                return;
            }

            GameObject point = Instantiate(m_PointPrefab);
            point.name = $"{name} point";

            point.transform.parent = m_PointsParent;

            if (m_LineMode == LineMode.Points)
            {
                if (m_Points.Count == 0)
                {
                    point.transform.position = transform.forward + transform.position;
                }
                else if (m_Points.Count == 1)
                {
                    point.transform.position = transform.forward + m_Points[0].position;
                }
                else
                {
                    Vector3 direction = (m_Points[m_Points.Count - 1].position - m_Points[m_Points.Count - 2].position).normalized;
                    point.transform.position = direction + m_Points[m_Points.Count - 1].position;
                }

                m_Points.Add(point.transform);
            }
            else
            {
                point.name = point.name + " Line";

                if (Selection.count > 0 && Selection.activeObject is GameObject selectPoint)
                {
                    bool isFindPoint = false;

                    for (int i = 0; i < m_Lines.Count; i++)
                    {
                        if (m_Lines[i].GetPointCount() == 0) continue;

                        Transform lastPoint = m_Lines[i].GetLastPoint();

                        if (lastPoint == null) continue;

                        if (lastPoint == selectPoint.transform)
                        {
                            isFindPoint = true;
                            point.transform.position = selectPoint.transform.forward + selectPoint.transform.position;
                            m_Lines[i].AddPoint(point.transform);
                            break;
                        }
                    }

                    if (isFindPoint == false)
                    {
                        List<Transform> newPoints = new List<Transform>();

                        newPoints.Add(selectPoint.transform);
                        newPoints.Add(point.transform);

                        point.transform.position = selectPoint.transform.forward + selectPoint.transform.position;

                        m_Lines.Add(new Line(newPoints));
                    }
                }
                else
                {
                    if (m_Lines.Count > 0)
                    {
                        if (m_Lines[m_Lines.Count - 1].GetPointCount() == 0) return;

                        Transform lastPoint = m_Lines[m_Lines.Count - 1].GetLastPoint();

                        if (lastPoint == null) return;

                        point.transform.position = 2 * lastPoint.forward + lastPoint.position;
                        m_Lines.Add(new Line(point.transform));
                    }
                    else
                    {
                        point.transform.position = transform.position + transform.forward;
                        Debug.Log(point);
                        m_Lines.Add(new Line(point.transform));
                    }
                }
            }

            Selection.activeObject = point;
        }

        [Button("Divide")]
        public void Divide()
        {
            if (Selection.count == 0) return;
            if (Selection.count > 2) return;

            GameObject[] points = Selection.gameObjects;

            for(int i = 0; i < m_Lines.Count; i++)
            {
                if (m_Lines[i].GetPointCount() == 0) continue;

                Transform[] linePoints = m_Lines[i].GetPoints();

                for (int j = 0; j < linePoints.Length; j++)
                {
                    if (points[0].transform == linePoints[j])
                    {
                        int pointIndex = -1;

                        if (points[1].transform == linePoints[j + 1])
                            pointIndex = j + 1;
                        else if (points[1].transform == linePoints[j - 1])
                            pointIndex = j - 1;

                        if (pointIndex > -1)
                        {
                            float distence = Vector3.Distance(points[0].transform.position, points[1].transform.position) / 2;
                            Vector3 direction = (points[1].transform.position - points[0].transform.position).normalized;

                            Vector3 position = points[0].transform.position + direction * distence;

                            GameObject newPoint = CreatPoint(position, m_PointsParent);

                            if (j > pointIndex)
                                m_Lines[i].AddPointByIndex(j, newPoint.transform);
                            else
                                m_Lines[i].AddPointByIndex(pointIndex, newPoint.transform);

                            newPoint.name = newPoint.name + " Divie"; 

                            Selection.activeObject = newPoint;

                            Debug.Log("New Point!");
                            return;
                        }
                    }
                }
            }
        }

        private GameObject CreatPoint(Vector3 position, Transform parent)
        {
            GameObject point = Instantiate(m_PointPrefab);
            point.name = $"{name} point";

            point.transform.position = position;
            point.transform.parent = parent;

            return point;
        }

        [Button("Add point")]
        private void AddPoint()
        {
            GameObject[] points = Selection.gameObjects;

            for (int i = 0; i < points.Length; i++)
                m_Points.Add(points[i].transform);
        }

        [Button("Convert Points to Line")]
        public void ConverPointsToLine()
        {
            List<Transform> newPoints = new List<Transform>();

            for(int i = 0; i < m_Points.Count; i++)
            {
                if (m_Points[i] == null)
                {
                    Debug.LogError($"{name} fance repitor have void Points!");
                    continue;
                }

                newPoints.Add(m_Points[i]);
            }

            m_Lines.Add(new Line(newPoints));
        }

        [Button("Remove point")]
        private void RemovePoint()
        {
            GameObject[] points = Selection.gameObjects;

            for (int i = 0; i < points.Length; i++)
            {
                for (int j = 0; j < m_Points.Count; j++)
                {
                    if (points[i].transform == m_Points[j])
                    {
                        m_Points.Remove(points[i].transform);
                        DestroyImmediate(points[i]);
                    }
                }
            }

            RemoveVoidPoints();
        }

        [Button("Remove all points")]
        public void RemoveAllPoints()
        {
            m_Points.Clear();

            Debug.Log("All Points removed!");
        }

        public void RemoveAllLines()
        {
            m_Lines.Clear();

            Debug.Log("All Lines removed!");
        }

        [Button("Remove void points")]
        public void RemoveVoidPoints()
        {
            int count = m_Points.Count;

            List<int> indexRemovedPoint = new List<int>();

            while (count > 0)
            {
                for (int i = 0; i < m_Points.Count; i++, count--)
                {
                    if (m_Points[i] == null)
                        indexRemovedPoint.Add(i);
                }

                for (int i = 0; i < indexRemovedPoint.Count; i++)
                {
                    m_Points.RemoveAt(indexRemovedPoint[i]);
                }
            }

            Debug.Log($"{indexRemovedPoint.Count} Points Removed!");
        }

        [Button("Remove no valit points in Line")]
        public void RemoveNoValitPointsInLine()
        {
            int count = m_Lines.Count;
            while (count > 0)
            {
                for (int i = 0; i < m_Lines.Count; i++, count--)
                {
                    if (m_Lines[i].GetPointCount() == 0)
                    {
                        continue;
                    }

                    Transform[] points = m_Lines[i].GetPoints();

                    int newCount = points.Length;

                    while (newCount > 0)
                    {
                        for (int j = 0; j < points.Length; j++, newCount--)
                        {
                            if (points[j] == null)
                            {
                                m_Lines[i].RemovePoint(points[j]);
                            }
                        }
                    }
                }
            }
        }

        [Button("Destroy all points")]
        private void DestroyAllPoints()
        {
            while (m_PointsParent.childCount > 0)
            {
                for (int i = 0; i < m_PointsParent.childCount; i++)
                    DestroyImmediate(m_PointsParent.GetChild(i).gameObject);
            }

            RemoveAllPoints();
            RemoveAllLines();

            Debug.LogWarning("All Points Destroy!");
        }

        [Button("Generate")]
        public void GenerateFances()
        {
            m_GenerateInRealTime = false;

            Generate();
        }

        [Button("Destroy Fences")]
        public void DestroyAllFance()
        {
            if (m_FencesObject.Count == 0) return;

            while (m_FencesParent.childCount > 0)
            {
                for (int i = 0; i < m_FencesObject.Count; i++)
                {
                    if (m_FencesObject[i] == null) continue;
                    DestroyImmediate(m_FencesObject[i]);
                }

                m_FencesObject.Clear();

                if (m_FencesParent.childCount > 0)
                    break;
            }
        }

        [Button("Conver old points to new")]
        public void ConverOldPointsToNew()
        {
            GameObject parent = new GameObject($"{name} parent NEW");
            parent.transform.parent = transform;

            if(m_LineMode == LineMode.Points)
            {
                int count = m_Points.Count;
                List<Transform> newPoints = new List<Transform>();

                while(count > 0)
                {
                    for(int i = 0; i < m_Points.Count; i++, count--)
                    {
                        if (m_Points[i] == null)
                        {
                            Debug.LogError($"{name} fance repitor have void Points!");
                            continue;
                        }

                        newPoints.Add(CreatPoint(m_Points[i].position, parent.transform).transform);
                    }
                }

                m_PointsParent = parent.transform;
                m_Points = newPoints;

                Debug.Log(newPoints.Count);
            }
        }

        private void Generate()
        {
            if (m_FencesParent == null)
            {
                GameObject fenceParent = new GameObject("Fence Parent");
                m_FencesParent = fenceParent.transform;
            }

            if (m_Fences == null)
            {
                Debug.LogError($"{name} Fances in null!");
                return;
            }

            ColculateFenceCount();

            if (m_FanceCount <= 0)
            {
                Debug.LogError($"{name} Fance Count 0!");
                return;
            }

            DestroyAllFance();

            while (m_FencesParent.childCount < m_FanceCount)
            {
                if (m_LineMode == LineMode.Points)
                {
                    for (int i = 1; i < m_Points.Count; i++)
                        AddFence(m_Points[i - 1].position, m_Points[i].position);
                }
                else
                {
                    for(int i = 0; i < m_Lines.Count; i++)
                    {
                        Transform[] points = m_Lines[i].GetPoints();

                        for(int j = 1; j < points.Length; j++)
                        {
                            AddFence(points[j - 1].position, points[j].position);
                        }
                    }
                }
            }
        }
        
        private void AddFence(Vector3 startPoint, Vector3 endPoint)
        {
            float distence = Vector3.Distance(startPoint, endPoint);
            Vector3 direction = (endPoint - startPoint).normalized;
            int fenceCount = (int)(distence / m_FenceLength);

            for (int i = 0; i < fenceCount; i++)
            {
                for (int a = 0; a < m_Fences.Length; a++)
                {
                    if (m_Fences[a] == null || m_Fences[a].GetPrefab() == null || m_Fences[a].GetWeight() <= 0)
                    {
                        Debug.LogError($"{name} Fances with index {a} on valit!");
                        return;
                    }

                    int fanceWeight = Random.Range(1, 101);

                    if (m_Fences[a].GetWeight() * 100 >= fanceWeight)
                    {
                        Vector3 poistion = startPoint + direction * (m_FenceLength * i);

                        GameObject fence = Instantiate(m_Fences[a].GetPrefab(), m_FencesParent);
                        fence.transform.position = ColculatePosition(poistion);

                        Quaternion rotationWithGround = ColculateRotationAnglOnGround(poistion, direction);
                        fence.transform.localRotation = rotationWithGround;

                        if (m_GenerateInRealTime == false)
                        {
                            Quaternion rotation = ColculateRotation(m_Fences[a], fence.transform.localRotation.eulerAngles);
                            fence.transform.localRotation = rotation;
                        }

                        m_FencesObject.Add(fence);
                    }
                }
            }
        }

        private void ColculateFenceCount()
        {
            /*
            if (m_Points == null)
            {
                Debug.LogError("Points List is null!");
                return;
            }
            */

            if (m_FenceLength <= 0)
            {
                Debug.LogError("Fance Langht get no valid value!");
                return;
            }

            m_FanceCount = 0;

            if (m_LineMode == LineMode.Points)
            {
                for (int i = 1; i < m_Points.Count; i++)
                {
                    float distence = Vector3.Distance(m_Points[i - 1].position, m_Points[i].position);
                    m_FanceCount += (int)(distence / m_FenceLength);
                }
            }
            else
            {
                for (int i = 0; i < m_Lines.Count; i++)
                {
                    Transform[] points = m_Lines[i].GetPoints();

                    for (int j = 1; j < points.Length; j++)
                    {
                        if (points[j] == null) continue;

                        float distence = Vector3.Distance(points[j - 1].position, points[j].position);
                        m_FanceCount += (int)(distence / m_FenceLength);
                    }
                }
            }
        }

        private Vector3 ColculatePosition(Vector3 position)
        {
            float y = position.y;

            if (Physics.Raycast(position + new Vector3(0, 100, 0), Vector3.down, out RaycastHit hit, 200, m_GroundLayer))
                y = hit.point.y;

            return new Vector3(position.x, y, position.z);
        }

        private Quaternion ColculateRotation(Fence fance, Vector3 rotation)
        {
            float y = Random.Range(fance.GetRotationAngleY().x, fance.GetRotationAngleY().y);
            float z = Random.Range(fance.GetRotationAngleZ().x, fance.GetRotationAngleZ().y);

            Quaternion newRotation = Quaternion.Euler(rotation.x, rotation.y + y, rotation.z + z);

            return newRotation;
        }

        private Quaternion ColculateRotationAnglOnGround(Vector3 position, Vector3 direction)
        {
            Vector3 newDirection = direction;

            Vector3 rayPoistion = position + new Vector3(0, 100, 0);
            Vector3 rayPoistion2 = position + direction * m_FenceLength + new Vector3(0, 100, 0);

            if (Physics.Raycast(rayPoistion, Vector3.down, out RaycastHit hit, 200, m_GroundLayer))
                if (Physics.Raycast(rayPoistion2, Vector3.down, out RaycastHit hit2, 200, m_GroundLayer))
                    newDirection = (hit2.point - hit.point).normalized;

            Quaternion newRotation = Quaternion.LookRotation(newDirection);

            return newRotation;
        }

        private void DrowLine(Vector3 start, Vector3 end)
        {
            float distence = Vector3.Distance(start, end);

            if (distence % m_FenceLength == 0 || distence % m_FenceLength < 0.2f)
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.red;

            Gizmos.DrawLine(start, end);
        }

        private void OnDrawGizmos()
        {
            if (Application.IsPlaying(gameObject)) return;

            if (m_LineMode == LineMode.Points)
            {
                if (m_Points == null) return;
                if (m_Points.Count == 0) return;

                for (int i = 0; i < m_Points.Count; i++)
                {
                    if (m_Points[i] == null)
                    {
                        Debug.LogError($"{name} fance repitor have void Points!");
                        continue;
                    }

                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireCube(m_Points[i].position, Vector3.one * 0.5f);

                    if (i == 0) continue;

                    DrowLine(m_Points[i - 1].position, m_Points[i].position);
                }
            }
            else
            {
                if (m_Lines == null) return;
                if (m_Lines.Count == 0) return;

                for(int i = 0; i < m_Lines.Count; i++)
                {
                    if (m_Lines.Count == 0) continue;

                    Transform[] points = m_Lines[i].GetPoints();

                    for(int j = 0; j < points.Length; j++)
                    {
                        if (points[j] == null) continue;

                        Gizmos.color = Color.magenta;
                        Gizmos.DrawWireCube(points[j].position, Vector3.one * 0.5f);

                        if (j == 0) continue;

                        if (points[j - 1] == null) continue;

                        DrowLine(points[j - 1].position, points[j].position);
                    }
                }
            }
        }
    }
}

#endif