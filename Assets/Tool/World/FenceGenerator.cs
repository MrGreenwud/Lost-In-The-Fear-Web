using UnityEngine;
using NaughtyAttributes;

#if UNITY_EDITOR

namespace BCTSTool.World
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(PointEditor))]
    public class FenceGenerator : MonoBehaviour
    {
        public enum Mode
        {
            Edit,
            Real_Time,
        }

        [SerializeField] private Mode m_GenerateMode;

        [Space(10)]

        [SerializeField] private LayerMask m_GroundLayer;

        [Space(10)]

        [SerializeField] private float m_FenceLength;
        [SerializeField] private Fence[] m_Fences;

        [SerializeField] private Transform m_FencesParent;

        private PointEditor m_PointEditor;
        private int m_FanceCount;

        private void OnValidate()
        {
            if (m_PointEditor == null)
                m_PointEditor = GetComponent<PointEditor>();

            if(m_GenerateMode == Mode.Real_Time)
            {
                m_PointEditor.OnAddPoint += Generate;
                m_PointEditor.OnMovePoint += Generate;
            }
            else
            {
                m_PointEditor.OnAddPoint -= Generate;
                m_PointEditor.OnMovePoint -= Generate;
            }
        }

        [Button("Generate")]
        private void Generate()
        {
            if (m_FencesParent == null)
            {
                GameObject fenceParent = new GameObject("Fence Parent");
                m_FencesParent = fenceParent.transform;
            }

            if(m_PointEditor == null)
                m_PointEditor = GetComponent<PointEditor>();

            DestroyAllFance();

            ColculateFanceCount();

            if (m_FanceCount == 0) return;

            Line[] lines = m_PointEditor.GetLines();

            for (int i = 0; i < lines.Length; i++)
            {
                Point[] points = lines[i].GetPoints();

                for (int j = 1; j < points.Length; j++)
                    AddFence(points[j - 1].Position, points[j].Position);
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

                        Quaternion rotation = ColculateRotation(m_Fences[a], fence.transform.localRotation.eulerAngles);
                        fence.transform.localRotation = rotation;
                    }
                }
            }
        }

        public void DestroyAllFance()
        {
            m_FanceCount = 0;
            
            if (m_FencesParent.childCount == 0) return;

            while (m_FencesParent.childCount > 0)
            {
                for (int i = 0; i < m_FencesParent.childCount; i++)
                    DestroyImmediate(m_FencesParent.GetChild(i).gameObject);
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

        private void ColculateFanceCount()
        {
            Line[] lines = m_PointEditor.GetLines();

            for (int i = 0; i < lines.Length; i++)
            {
                Point[] points = lines[i].GetPoints();

                for (int j = 1; j < points.Length; j++)
                {
                    float distence = Vector3.Distance(points[j - 1].Position, points[j].Position);
                    m_FanceCount += (int)(distence / m_FenceLength);
                }
            }
        }
    }
}

#endif