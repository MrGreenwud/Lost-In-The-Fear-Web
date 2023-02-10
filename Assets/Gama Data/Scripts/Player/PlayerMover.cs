using ModestTree.Util;
using System;
using UnityEngine;

public class PlayerMover
{
    private PlayerController m_PlayerController;

    public Action<bool> OnMove;

    public PlayerMover(PlayerController playerController)
    {
        m_PlayerController = playerController;
    }

    public void Move(float moveSpeed)
    {
        float x = m_PlayerController.InputHandler.GetMoveAxis().x;
        float y = m_PlayerController.InputHandler.GetMoveAxis().y;

        Vector3 moveDirection = m_PlayerController.transform.right * x + m_PlayerController.transform.forward * y;
        m_PlayerController.CharacterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }
}
