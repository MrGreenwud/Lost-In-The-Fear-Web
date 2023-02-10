using UnityEngine;

public class PlayerGravity
{
    private PlayerController m_PlayerController;
    private CharacterController m_CharacterController;
    
    private Vector3 m_BoxSize;
    private Vector3 m_Position;

    private Vector3 m_Velocity;

    public PlayerGravity(PlayerController playerController)
    {
        m_PlayerController = playerController;
        m_CharacterController = m_PlayerController.CharacterController;

        m_BoxSize = new Vector3(m_CharacterController.radius, m_CharacterController.radius, m_CharacterController.radius); 
    }

    public bool ChackGround()
    {
        m_Position = m_PlayerController.transform.position - new Vector3(0, m_CharacterController.height / 2 - m_BoxSize.y + m_CharacterController.skinWidth * 2, 0);

        if (Physics.CheckBox(m_Position, m_BoxSize, Quaternion.identity, m_PlayerController.GetGroundLayer()))
            return true;
        else
            return false;
    }

    public void Fall()
    {
        if (ChackGround() == true)
            m_Velocity.y = 0;

        m_Velocity.y += m_PlayerController.GetGravity();
        m_CharacterController.Move(m_Velocity * Time.deltaTime);
    }

    public void OnDrawGizmos()
    {
        if (Application.isPlaying == false) return;
        
        if (ChackGround() == true)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;

        Gizmos.DrawWireCube(m_Position, m_BoxSize * 2);
    }
}
