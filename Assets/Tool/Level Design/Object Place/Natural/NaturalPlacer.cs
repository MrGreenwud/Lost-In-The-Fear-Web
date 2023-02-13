using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BCTSTool.World
{
    [Serializable]
    public class NaturalInstance
    {
        [SerializeField] private GameObject m_Prefab;
        [SerializeField][Range(0, 1)] private float m_Weigth;

        [SerializeField] private Vector2 m_RotationX;
        [SerializeField] private Vector2 m_RotationY;
        [SerializeField] private Vector2 m_RotationZ;

        public GameObject GetPrefab() => m_Prefab;
        public float GetWeight() => m_Weigth;

        public Vector2 GetRotationX() => m_RotationX;
        public Vector2 GetRotationY() => m_RotationY;
        public Vector2 GetRotationZ() => m_RotationZ;
    }

    [ExecuteInEditMode]
    public class NaturalPlacer : MonoBehaviour
    {
        public uint NaturalsCount { get; private set; }
        [SerializeField] private LayerMask m_GroundLayer;

        private Transform m_TreesParent;

        private void OnEnable()
        {
            if (m_TreesParent == null)
            {
                GameObject treesParent = new GameObject("Trees");
                m_TreesParent = treesParent.transform;
            }
        }

        private bool CheckValidate(float radius, Naturals natuals, Vector2Int count)
        {
            if (radius < 1)
            {
                Debug.LogError("Radius should not be negative!");
                return false;
            }

            if (natuals == null)
            {
                return false;
            }
            else if (natuals.GetNaturals() == null)
            {
                return false;
            }

            if(count.x < 0)
            {
                return false;
            }

            if(count.y < count.x)
            {
                return false;
            }

            return true;
        }

        public void GenerateByCount(float radius, Vector3 position, Naturals naturals, Vector2Int count)
        {
            NaturalInstance[] tempNaturals = naturals.GetNaturals();
            int naturalsCount = Random.Range(count.x, count.y);

            while (naturalsCount > 0)
            {
                for (int i = 0; i < tempNaturals.Length; i++)
                {
                    if (tempNaturals[i].GetPrefab() == null)
                    {
                        return;
                    }

                    Vector3 rayPosition = GenerateRandomPoistion(radius, position);
                    Quaternion rotation = GenerateRandomRotaion(tempNaturals[i]);

                    if (Physics.Raycast(rayPosition, Vector3.down, out RaycastHit hit, 200, m_GroundLayer))
                    {
                        GameObject tree = Instantiate(tempNaturals[i].GetPrefab(), hit.point, rotation, m_TreesParent);
                        NaturalsCount++;
                        naturalsCount--;
                    }
                }
            }
        }

        public void GenerateByDensity(float radius, Vector3 position, Naturals naturals, float density)
        {
            if (density == 0) return;

            NaturalInstance[] tempNaturals = naturals.GetNaturals();
            int naturalsCount = (int)((radius * radius) / density);

            for (int i = 0; i < naturalsCount; i++)
            {
                int s = Random.Range(0, tempNaturals.Length - 1);

                if (tempNaturals[s].GetPrefab() == null)
                {
                    return;
                }

                Vector3 rayPosition = GenerateRandomPoistion(radius, position);
                Quaternion rotation = GenerateRandomRotaion(tempNaturals[s]);

                bool isGenerate = true;

                if (Physics.Raycast(rayPosition, Vector3.down, out RaycastHit hit, 200, m_GroundLayer))
                {
                    RaycastHit[] hits = Physics.SphereCastAll(hit.point, density, Vector3.up * 0.1f);

                    for (int j = 0; j < hits.Length; j++)
                    {
                        if (hits[j].collider.TryGetComponent<Natural>(out Natural natural))
                        {
                            isGenerate = false;
                            break;
                        }
                    }

                    if (isGenerate == false) break;

                    GameObject tree = Instantiate(tempNaturals[s].GetPrefab(), hit.point, rotation, m_TreesParent);
                    NaturalsCount++;
                }
            }
        }

        public Vector3 GenerateRandomPoistion(float radius, Vector3 point)
        {
            float x = Random.Range(-radius, radius);
            float z = Random.Range(-radius, radius);

            Vector3 rayPositon = point + new Vector3(x, 20, z);

            return rayPositon;
        }

        public Quaternion GenerateRandomRotaion(NaturalInstance naturals)
        {
            Vector2 rotationX = naturals.GetRotationX();
            Vector2 rotationY = naturals.GetRotationY();
            Vector2 rotationZ = naturals.GetRotationZ();

            float randomRotationX = Random.Range(rotationX.x, rotationX.y);
            float randomRotationY = Random.Range(rotationY.x, rotationY.y);
            float randomRotationZ = Random.Range(rotationZ.x, rotationZ.y);

            Quaternion rotation = Quaternion.Euler(randomRotationX, randomRotationY, randomRotationZ);

            return rotation;
        }

        public void Remove(float radius, Vector3 position, Vector2Int count)
        {
            RaycastHit[] hits = Physics.SphereCastAll(position, radius, Vector3.up * 0.01f);
            Queue<GameObject> Trees = new Queue<GameObject>();

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.TryGetComponent<Natural>(out Natural tree))
                    Trees.Enqueue(hits[i].collider.gameObject);
            }

            int removedTreeCount = Random.Range(count.x, count.y);

            if(removedTreeCount > Trees.Count)
                removedTreeCount = Trees.Count;

            for(int i = 0; i < removedTreeCount; i++)
            {
                DestroyImmediate(Trees.Dequeue());
                NaturalsCount--;
            }
        }

        private void OnDestroy()
        {
            if (m_TreesParent.childCount == 0)
                DestroyImmediate(m_TreesParent.gameObject);
        }
    }
}
