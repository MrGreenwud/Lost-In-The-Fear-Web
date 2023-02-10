using UnityEngine;
using Zenject;

public class StairsQuest : QuestCompliter
{
    [Inject] private readonly PlayerMoverByWay m_PlayerMoverByWay;

    [SerializeField] private GameObject m_Stairs;
    [SerializeField] private GameObject m_OutLineStairs;

    [Space(10)]

    [SerializeField] private GameObject m_Steps;

    [Space(10)]

    [SerializeField] private Transform m_PlayerTeleportPosition;

    private void Start()
    {
        m_OutLineStairs.SetActive(false);
    }

    public override void Interact()
    {
        if (IsComplite == false)
            base.Interact();
        else
            Complite();
    }

    public void PutSteps()
    {
        if(m_Steps == null)
        {
            Debug.LogError("Steps field is null!");
            return;
        }

        m_Steps.SetActive(true);
    }

    public void PutStairs()
    {
        m_Stairs.SetActive(true);
        HideOutline();
    }

    public void ShowOutline()
    {
        if (m_MainAction.IsComplite == true) return;
        m_OutLineStairs.SetActive(true);
    }

    public void HideOutline()
    {
        m_OutLineStairs.SetActive(false);
    }

    private void Complite()
    {
        Debug.Log("Stairs Quest is Complite!");
        
        if(m_PlayerTeleportPosition == null)
        {
            Debug.Log("Player Teleport Position field is null!");
            return;
        }

        m_PlayerMoverByWay.Teleport(m_PlayerTeleportPosition.position, 0.1f);
    }
}
