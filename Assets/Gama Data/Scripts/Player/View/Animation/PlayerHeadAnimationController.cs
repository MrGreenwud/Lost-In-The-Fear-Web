using UnityEngine;
using Zenject;

[RequireComponent(typeof(Animator))]
public class PlayerHeadAnimationController : MonoBehaviour
{
    [Inject] private readonly PlayerController m_PlayerController;

    private Animator m_Animator;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        m_Animator.SetBool("Is Move", m_PlayerController.IsMove);
        m_Animator.SetBool("Is Run", m_PlayerController.IsRun);
        m_Animator.SetBool("Is Crouch", m_PlayerController.IsCrouch);
        m_Animator.SetBool("Is Jump", m_PlayerController.IsJump);
    }
}
