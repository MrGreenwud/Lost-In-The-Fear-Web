using UnityEngine;

namespace BCTSTool.World
{
    [CreateAssetMenu(fileName = "Trees", menuName = "BSTSTool/World")]
    public class Naturals : ScriptableObject
    {
        [SerializeField] private NaturalInstance[] m_Natural;

        public NaturalInstance[] GetNaturals()
        {
            NaturalInstance[] newTrees = new NaturalInstance[m_Natural.Length];

            for (int i = 0; i < m_Natural.Length; i++)
                newTrees[i] = m_Natural[i];

            return newTrees;
        }
    }
}
