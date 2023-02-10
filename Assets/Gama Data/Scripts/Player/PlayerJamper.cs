using UnityEngine;

public class PlayerJamper
{
    public float JumpTCofisent { get; private set; }
    
    private readonly PlayerController m_PlayerController;

    private readonly float m_JumpOverSpeed;
    private readonly float m_MaxObstacleHight;
    private readonly float m_MaxObstacleLong;
    private readonly LayerMask m_ObstacleLayer;

    private Transform m_Transform;
    
    private Vector3 m_StartPosition;
    private Vector3 m_CenterPosition;
    private Vector3 m_EndPosition;

    public PlayerJamper(PlayerController playerController)
    {
        m_PlayerController = playerController;
        m_Transform = m_PlayerController.transform;

        m_JumpOverSpeed = m_PlayerController.GetJumpOverSpeed();
        m_MaxObstacleHight = m_PlayerController.GetMaxObstacleHight();
        m_MaxObstacleLong = m_PlayerController.GetMaxObstacleLong();
        m_ObstacleLayer = m_PlayerController.GetObstacleLayer();
    }

    public void Jump()
    {
        JumpTCofisent += Time.deltaTime * m_JumpOverSpeed;
        m_Transform.position = ColculateJumpOverPosition(m_StartPosition, m_CenterPosition, m_EndPosition, JumpTCofisent) + new Vector3(0, 1, 0);
    }

    public bool CheckJumpOver()
    {
        Vector3 obstacleCheckerPosition = new Vector3(m_Transform.position.x, m_Transform.position.y - 0.5f, m_Transform.position.z);

        if (Physics.Raycast(obstacleCheckerPosition, m_Transform.forward, out RaycastHit hitHight, m_MaxObstacleLong / 2, m_ObstacleLayer))
        {
            if (hitHight.collider.GetComponent<JumpObstacle>() == null) return false;
            
            Vector3 obstecleLongChacker = obstacleCheckerPosition + m_Transform.forward * m_MaxObstacleLong;

            if (Physics.Raycast(obstecleLongChacker, -m_Transform.forward, out RaycastHit hitLong, 10, m_ObstacleLayer))
            {
                if (hitLong.collider.GetComponent<JumpObstacle>() == null) return false;

                Vector3 jumpPosition = hitLong.point + m_Transform.forward * 0.5f + new Vector3(0, m_MaxObstacleHight, 0);

                if (Physics.Raycast(jumpPosition, Vector3.down, out RaycastHit hitJump, 100, m_ObstacleLayer))
                {
                    Vector3 obstecleCenterChacker = hitLong.point - m_Transform.forward * (Vector3.Distance(hitHight.point, hitLong.point) / 2f) + new Vector3(0, m_MaxObstacleHight, 0);

                    if (hitJump.collider.GetComponent<JumpObstacle>() != null) return false;

                    if (Physics.Raycast(obstecleCenterChacker, Vector3.down, out RaycastHit hitCenter, 100, m_ObstacleLayer))
                    {
                        if (hitCenter.collider.GetComponent<JumpObstacle>() == null) return false;

                        m_StartPosition = m_Transform.position - new Vector3(0, 1, 0);
                        m_CenterPosition = hitCenter.point;
                        m_EndPosition = hitJump.point;

                        if(m_PlayerController.IsJump == false)
                            JumpTCofisent = 0;

                        return true;
                    }
                }
            }
        }

        return false;
    }

    private Vector3 ColculateJumpOverPosition(Vector3 startPostion, Vector3 centerPosition, Vector3 endPosition, float t)
    {
        Vector3 v1 = Vector3.Lerp(startPostion, centerPosition, t);
        Vector3 v2 = Vector3.Lerp(centerPosition, endPosition, t);
        Vector3 v3 = Vector3.Lerp(v1, v2, t);

        return v3;
    }

    private void DrowJumpOverCurve(Vector3 startPostion, Vector3 centerPosition, Vector3 endPosition)
    {
        Vector3 firstPosition = startPostion;

        for (int i = 0; i < 20; i++)
        {
            float parameter = (float)i / 20;
            Vector3 point = ColculateJumpOverPosition(startPostion, centerPosition, endPosition, parameter);

            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(firstPosition, point);

            if (parameter == 0.5f || parameter == 0 || parameter > 0.9f)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawWireSphere(point, 0.2f);
            }

            firstPosition = point;
        }
    }

#if UNITY_EDITOR

    public void OnDrawGizmos()
    {
        Vector3 obstacleCheckerPosition = new Vector3(m_Transform.position.x, m_Transform.position.y - 0.5f, m_Transform.position.z);

        if (Physics.Raycast(obstacleCheckerPosition, m_Transform.forward, out RaycastHit hitHight, m_MaxObstacleLong / 2, m_ObstacleLayer))
        {
            if (hitHight.collider.GetComponent<JumpObstacle>() == null) return;

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(obstacleCheckerPosition, hitHight.point);

            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawWireDisc(hitHight.point, hitHight.normal, 0.5f);

            UnityEditor.Handles.color = Color.magenta;
            UnityEditor.Handles.DrawWireDisc(m_Transform.position - new Vector3(0, 1, 0), Vector3.up, 0.5f);

            Vector3 obstecleLongChacker = obstacleCheckerPosition + m_Transform.forward * m_MaxObstacleLong;

            if (Physics.Raycast(obstecleLongChacker, -m_Transform.forward, out RaycastHit hitLong, 10, m_ObstacleLayer))
            {
                if (hitLong.collider.GetComponent<JumpObstacle>() == null) return;

                Gizmos.color = Color.red;
                Gizmos.DrawLine(obstecleLongChacker, hitLong.point);

                UnityEditor.Handles.color = Color.cyan;
                UnityEditor.Handles.DrawWireDisc(hitLong.point, hitLong.normal, 0.5f);

                Vector3 jumpPosition = hitLong.point + m_Transform.forward * 0.5f + new Vector3(0, m_MaxObstacleHight, 0);

                if (Physics.Raycast(jumpPosition, Vector3.down, out RaycastHit hitJump, 100, m_ObstacleLayer))
                {
                    if (hitJump.collider.GetComponent<JumpObstacle>() != null) return;

                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(jumpPosition, hitJump.point);

                    UnityEditor.Handles.color = Color.magenta;
                    UnityEditor.Handles.DrawWireDisc(hitJump.point, hitJump.normal, 0.5f);

                    Vector3 obstecleCenterChacker = hitLong.point - m_Transform.forward * (Vector3.Distance(hitHight.point, hitLong.point) / 2f) + new Vector3(0, m_MaxObstacleHight, 0);

                    if (Physics.Raycast(obstecleCenterChacker, Vector3.down, out RaycastHit hitCenter, 100, m_ObstacleLayer))
                    {
                        if (hitCenter.collider.GetComponent<JumpObstacle>() == null) return;

                        Gizmos.color = Color.green;
                        Gizmos.DrawLine(obstecleCenterChacker, hitCenter.point);

                        UnityEditor.Handles.color = Color.magenta;
                        UnityEditor.Handles.DrawWireDisc(hitCenter.point, hitCenter.normal, 0.5f);

                        DrowJumpOverCurve(m_Transform.position - new Vector3(0, 1, 0), hitCenter.point, hitJump.point);
                    }
                }
            }
        }
    }

#endif

}