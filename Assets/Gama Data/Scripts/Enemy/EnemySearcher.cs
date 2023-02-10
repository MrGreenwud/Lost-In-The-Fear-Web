using UnityEngine;
using UnityEngine.AI;

public class EnemySearcher
{
    private readonly BaseEnemyController m_Controller;
    private readonly NavMeshAgent m_NavMeshAgent;
    private readonly float m_SearchDistance;

    public EnemySearcher(BaseEnemyController controller)
    {
        m_Controller = controller;
        m_NavMeshAgent = m_Controller.NavMeshAgent;
        m_SearchDistance = m_Controller.GetSearchDistence();
    }

    public virtual void Search()
    {
        Vector3 lastPathPoint = m_Controller.transform.position;

        if (m_NavMeshAgent.path.corners.Length > 0)
            lastPathPoint = m_NavMeshAgent.path.corners[m_NavMeshAgent.path.corners.Length - 1];

        if (Vector3.Distance(lastPathPoint, m_Controller.transform.position) < 2)
            FindPosition();
    }

    public void ReSearch()
    {
        m_NavMeshAgent.ResetPath();
        FindPosition();
    }

    private void FindPosition()
    {
        float x = Random.Range(m_Controller.Target.position.x,
                m_Controller.Target.position.x
                + Random.Range(-m_SearchDistance, m_SearchDistance));

        float z = Random.Range(m_Controller.Target.position.z,
            m_Controller.Target.position.z
            + Random.Range(-m_SearchDistance, m_SearchDistance));

        m_Controller.EnemyMover.Move(new Vector3(x, m_Controller.Target.position.y, z), m_Controller.GetWalkSpeed());
    }
}
