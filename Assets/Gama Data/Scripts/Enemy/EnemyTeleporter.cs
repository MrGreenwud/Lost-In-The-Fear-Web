using System.Collections.Generic;
using UnityEngine;

public class EnemyTeleporter
{
    public float TeleportTimer { get; protected set; }
    
    private readonly GhostEnemyController m_Controller;
    private readonly Transform m_Transform;
    private readonly Transform m_Target;

    public EnemyTeleporter(GhostEnemyController controller)
    {
        m_Controller = controller;
        m_Transform = controller.transform;
        m_Target = controller.Target;
        TeleportTimer = controller.GetTeleportCoolDown();
    }

    public virtual void Teleport()
    {
        m_Transform.position = FindTeleportPosition();

        m_Transform.rotation = Quaternion.LookRotation(m_Controller.Target.position 
            - m_Controller.transform.position);

        TeleportTimer = m_Controller.GetTeleportCoolDown();
        
        m_Controller.EnemySearcher.ReSearch();
    }

    public virtual Vector3 FindTeleportPosition()
    {
        RaycastHit[] points = Physics.SphereCastAll(m_Target.position, m_Controller.GetTeleportDistence(),
            Vector3.up * 0.01f, m_Controller.GetTeleportPointLayer());

        List<Transform> teleportPoints = new List<Transform>();

        for (int i = 0; i < points.Length; i++)
        {
            if (points[i].collider.TryGetComponent<TeleportPoint>(out TeleportPoint teleportPoint))
                teleportPoints.Add(points[i].transform);
        }

        if (teleportPoints.Count > 0)
        {
            Vector3 farPoint = teleportPoints[0].position;
            float distenceToFarPoint = Vector3.Distance(farPoint, m_Target.position);

            for (int i = 1; i < teleportPoints.Count; i++)
            {
                float distenceToCheckPoint = Vector3.Distance(teleportPoints[i].position, m_Target.position);

                if (distenceToFarPoint < distenceToCheckPoint)
                {
                    farPoint = teleportPoints[i].position;
                    distenceToFarPoint = distenceToCheckPoint;
                }
            }

            return farPoint;
        }
        else
        {
            Debug.LogError("Don't find teleport points!");
            return Vector3.zero;
        }
    }

    public virtual void Update()
    {
        if (m_Controller.IsFollow == false)
            TeleportTimer -= Time.fixedDeltaTime;
        else
            TeleportTimer = m_Controller.GetTeleportCoolDown();
    }

#if UNITY_EDITOR

    public virtual void GizmosUpdate()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(m_Target.position, m_Controller.GetTeleportDistence());
    }

#endif
}