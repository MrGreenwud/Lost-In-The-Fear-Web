using System;
using UnityEngine;

public class HollyWoterDroper
{
    private readonly Protactor m_Protactor;
    private readonly Transform m_Transform;

    public Action OnDrop;

    public HollyWoterDroper(Protactor protactor)
    {
        m_Protactor = protactor;
        m_Transform = m_Protactor.transform;
    }

    public void Drop(float stanTime)
    {
        Vector3 boxSize = Vector3.one * m_Protactor.GetHollyWaterUseBoxSize() * 0.5f;

        RaycastHit[] hits = Physics.BoxCastAll(m_Transform.position, boxSize, m_Transform.forward,
            m_Transform.rotation, 0, m_Protactor.GetEnemyLayer());

        OnDrop?.Invoke();

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.TryGetComponent<EnemyController>(out EnemyController enemyController))
            {
                enemyController.ApplyStan(stanTime);
                m_Protactor.OnEnemyStan?.Invoke(enemyController, stanTime);
                Debug.Log(hits[i].collider.name + " is Hit!");
            }
        }

        m_Protactor.ResetPotactionParametor();
    }
}
