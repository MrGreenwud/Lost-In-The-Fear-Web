using UnityEngine;
using Zenject;

public class PlayerFoodStaps : FoodSteps
{
    [Inject] private readonly PlayerController m_PlayerController;

    private void OnEnable()
    {
        Debug.Log(m_PlayerController);
        Debug.Log(m_PlayerController.PlayerMover);
        m_PlayerController.PlayerMover.OnMove += Step;
    }

    private void OnDisable()
    {
        m_PlayerController.PlayerMover.OnMove -= Step;
    }
}
