using UnityEngine;

[RequireComponent(typeof(GhostEnemyController))]
public class GhostEnemyAnimationController : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;

    private GhostEnemyController m_GhostEnemyController;

    private const string c_IsRun = "IsRun";

    private void Awake()
    {
        if(m_Animator == null)
            Debug.LogError($"{name} + Animator is null!");

        m_GhostEnemyController = GetComponent<GhostEnemyController>();
    }

    private void Update()
    {
        m_Animator.SetBool(c_IsRun, m_GhostEnemyController.IsRun);
    }
}
