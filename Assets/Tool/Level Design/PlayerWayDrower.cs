using UnityEngine;
using BCTSTool.Editor;

public class PlayerWayDrower : MonoBehaviour
{
    [SerializeField] private Way[] m_Ways;

    private const float c_SphereRadius = 1;
    private const float c_ArrowDistence = 5;

    private void OnDrawGizmos()
    {
        if (m_Ways == null) return;

        for (int i = 0; i < m_Ways.Length; i++)
        {
            if (m_Ways[i] == null)
            {
                Debug.LogError($"Way with index {i} is null!");
                continue;
            }

            Vector3[] positions = m_Ways[i].GetPoints();

            for (int j = 0; j < positions.Length; j++)
            {
                Gizmos.color = m_Ways[i].GetWayColor();
                Gizmos.DrawWireSphere(positions[j], c_SphereRadius);

                if (positions.Length - 1 >= j + 1)
                {
                    Gizmos.DrawLine(positions[j], positions[j + 1]);

                    Vector3 direation = (positions[j + 1] - positions[j]).normalized;

                    float distence = Vector3.Distance(positions[j], positions[j + 1]);
                    int arrowCount = (int)(distence / c_ArrowDistence);

                    for (int a = 0; a < arrowCount; a++)
                        CustemGizmos.DrowArrow(positions[j] + direation * (c_ArrowDistence * a), direation);

                    if (m_Ways[i].GetIsReturn() == true)
                    {
                        Color arrowColor = Gizmos.color = m_Ways[i].GetWayColor();

                        arrowColor.g = arrowColor.g / 5;
                        arrowColor.r = arrowColor.r / 5;
                        arrowColor.b = arrowColor.b / 5;

                        Gizmos.color = arrowColor;

                        for (int a = 0; a < arrowCount; a++)
                            CustemGizmos.DrowArrow(positions[j] + direation * (c_ArrowDistence * a + 2.5f), -direation);
                    }

                }
            }
        }
    }
}