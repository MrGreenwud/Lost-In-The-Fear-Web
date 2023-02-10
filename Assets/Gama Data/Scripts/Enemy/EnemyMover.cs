using UnityEngine;
using UnityEngine.AI;

public class EnemyMover
{
    protected BaseEnemyController p_Controller;
    protected NavMeshAgent p_NavMeshAgent;

    public EnemyMover(BaseEnemyController controller)
    {
        p_Controller = controller;
        p_NavMeshAgent = p_Controller.NavMeshAgent;
    }

    public void Move(Vector3 target, float speed)
    {
        float distance = Vector3.Distance(p_Controller.transform.position, p_Controller.Target.position);
        
        if (distance < 5)
            p_NavMeshAgent.acceleration = 100;
        else
            p_NavMeshAgent.acceleration = 50;

        p_NavMeshAgent.speed = speed;

        p_NavMeshAgent.SetDestination(target);
    }

    private void DrowPath()
    {
        if (p_NavMeshAgent.hasPath == true)
        {
            Vector3 diraction = (p_Controller.Target.position - p_Controller.transform.position);
            float distance = Vector3.Distance(p_Controller.transform.position, p_Controller.Target.position);

            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(new Ray(p_Controller.transform.position, diraction * distance));

            for (int i = 0; i < p_NavMeshAgent.path.corners.Length; i++)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(p_NavMeshAgent.path.corners[i], 0.5f);

                if (i < p_NavMeshAgent.path.corners.Length - 1)
                {
                    if (Vector3.Distance(p_NavMeshAgent.path.corners[i], p_NavMeshAgent.path.corners[i + 1]) >= 10)
                        Gizmos.color = Color.red;
                    else if (Vector3.Distance(p_NavMeshAgent.path.corners[i], p_NavMeshAgent.path.corners[i + 1]) >= 5)
                        Gizmos.color = Color.yellow;
                    else
                        Gizmos.color = Color.green;

                    Gizmos.DrawLine(p_NavMeshAgent.path.corners[i], p_NavMeshAgent.path.corners[i + 1]);
                }
            }
        }
    }

#if UNITY_EDITOR

    public void OnDrowGizmos()
    {
        DrowPath();

        UnityEditor.Handles.color = Color.white;
        UnityEditor.Handles.DrawWireDisc(p_Controller.Target.position, Vector3.up, p_Controller.GetSearchDistence());
    }

#endif

}
