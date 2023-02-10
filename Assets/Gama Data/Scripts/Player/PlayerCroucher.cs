using UnityEngine;

public class PlayerCroucher
{
    private PlayerController m_PlayerController;
    private float m_StayHeight;

    public PlayerCroucher(PlayerController playerController)
    {
        m_PlayerController = playerController;
        m_StayHeight = m_PlayerController.CharacterController.height;
    }

    public void Crouch()
    {
        m_PlayerController.CharacterController.height = m_PlayerController.GetCrouchHeight();
    }

    public void Stay()
    {
        m_PlayerController.CharacterController.height = m_StayHeight;
        m_PlayerController.CharacterController.Move(m_PlayerController.transform.forward * 0.00001f);
    }
}
