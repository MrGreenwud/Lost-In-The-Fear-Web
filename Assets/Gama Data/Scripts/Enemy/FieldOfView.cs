using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] [Range(0, 360)] private float m_FOV = 90;
    [SerializeField] private float m_ViewDistence = 10;
    [SerializeField] private float m_StepCount = 30;

    [SerializeField] private float m_ReactionDistence = 2;

    [SerializeField] private LayerMask m_ObstacleLayer;

    public float GetViewDistence() => m_ViewDistence;

    public bool Check(Transform target)
    {
        Vector3 playerDiraction = target.position - transform.position;
        float distance = Vector3.Distance(transform.position, target.position);

        if (!Physics.Raycast(transform.position + new Vector3(0, 1, 0), playerDiraction, distance, m_ObstacleLayer))
        {
            if (Vector3.Angle(transform.forward, playerDiraction) < m_FOV / 2 && distance < m_ViewDistence)
                return true;

            if (distance < m_ReactionDistence)
                return true;
        }

        return false;
    }

#if UNITY_EDITOR

    private void DrowFOV()
    {
        UnityEditor.Handles.color = Color.white;

        UnityEditor.Handles.DrawWireArc(transform.position, Vector3.up, transform.forward, m_FOV / 2, m_ViewDistence);
        UnityEditor.Handles.DrawWireArc(transform.position, Vector3.up, transform.forward, -m_FOV / 2, m_ViewDistence);

        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireArc(transform.position, Vector3.up, transform.forward, 360, m_ReactionDistence);

        Vector3 left = transform.position + Quaternion.Euler(new Vector3(0, m_FOV / 2, 0)) * (transform.forward * m_ViewDistence);
        Vector3 right = transform.position + Quaternion.Euler(-new Vector3(0, m_FOV / 2, 0)) * (transform.forward * m_ViewDistence);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, left);
        Gizmos.DrawLine(transform.position, right);

        float stepAngleSize = m_FOV / m_StepCount;

        Gizmos.color = Color.cyan;
        for (int i = 1; i < m_StepCount; i++)
        {
            float angle = transform.rotation.y - m_FOV / 2 + stepAngleSize * i;

            Vector3 lastPosition = transform.position + Quaternion.Euler(-new Vector3(0, angle, 0)) * (transform.forward * m_ViewDistence);

            Vector3 diraction = lastPosition - transform.position;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, diraction, out hit, m_ViewDistence, m_ObstacleLayer))
                Gizmos.DrawLine(transform.position, hit.point);
            else
                Gizmos.DrawLine(transform.position, lastPosition);
        }
    }

    private void OnDrawGizmos()
    {
        DrowFOV();
    }

#endif
}
