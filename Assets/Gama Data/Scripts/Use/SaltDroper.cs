using System;
using UnityEngine;
using BCTSTool.Math;

public class SaltDroper
{
    private readonly Protactor m_Protactor;
    private readonly Transform m_Transform;
    private readonly GameObject m_Salt;

    public Action OnFallDown;

    private Vector3 m_FallStartPosition;
    public Vector3 FallEndPosition { get; private set; }

    private float m_StanTime;
    private float m_Radius;

    private float m_DropTCoficent;

    public SaltDroper(Protactor protactor)
    {
        m_Protactor = protactor;
        m_Transform = m_Protactor.transform;
        m_Salt = m_Protactor.GetSalt();
    }

    public void Update()
    {
        Fall();
    }

    public void Drop(float stanTime, float stanDistence)
    {
        if (m_Salt.activeInHierarchy == false)
        {
            FallEndPosition = CalculateSaltFallPosition();
            m_FallStartPosition = m_Transform.position;

            m_Salt.transform.position = m_Transform.position;
            m_DropTCoficent = 0;

            m_Salt.SetActive(true);

            m_StanTime = stanTime;
            m_Radius = stanDistence;
        }
    }

    private void Fall()
    {
        if (m_Salt.activeInHierarchy == true)
        {
            m_DropTCoficent += Time.deltaTime * m_Protactor.GetFallSpeed();

            m_Salt.transform.position = LerpCureve.ColculatePositions(m_FallStartPosition, FallEndPosition, m_DropTCoficent, m_Protactor.GetDropAmpletude());

            if (Physics.CheckBox(m_Salt.transform.position, Vector3.one * 0.25f, m_Salt.transform.rotation, m_Protactor.GetColitionDetection()))
            {
                ApplayStan();
                OnFallDown?.Invoke();
                m_Salt.SetActive(false);
                m_Protactor.ResetPotactionParametor();
            }
        }
    }

    private void ApplayStan()
    {
        RaycastHit[] hits = Physics.SphereCastAll(FallEndPosition, m_Radius, Vector3.up, 0, m_Protactor.GetEnemyLayer());

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.TryGetComponent<EnemyController>(out EnemyController enemyController))
            {
                enemyController.ApplyStan(m_StanTime);
                m_Protactor.OnEnemyStan?.Invoke(enemyController, m_StanTime);
                Debug.Log(hits[i].collider.name + " is Hit!");
            }
        }
    }

    private Vector3 CalculateSaltFallPosition()
    {
        float maxSaltDropDistence;

        RaycastHit wallHit;
        if (Physics.Raycast(m_Transform.position, m_Transform.forward, out wallHit, m_Protactor.GetMaxSaltDropDistence(), m_Protactor.GetColitionDetection()))
            maxSaltDropDistence = Vector3.Distance(m_Transform.position, wallHit.point);
        else
            maxSaltDropDistence = m_Protactor.GetMaxSaltDropDistence();

        Vector3 groundChackerPosition = new Vector3(m_Transform.position.x, m_Transform.position.y + 1, m_Transform.position.z) +
        m_Transform.forward * maxSaltDropDistence - m_Transform.forward * 0.2f;

        RaycastHit groundHit;
        if (Physics.Raycast(groundChackerPosition, Vector3.down, out groundHit, 10000, m_Protactor.GetGroundLayer()))
            return groundHit.point;

        return Vector3.zero;
    }

    public void OnDrawGizmos()
    {
        if (m_Salt.activeInHierarchy == true)
            LerpCureve.Draw(m_FallStartPosition, FallEndPosition, m_FallStartPosition, m_Protactor.GetDropAmpletude());

        Gizmos.color = Color.red;
        if (m_Salt.activeInHierarchy == true)
            Gizmos.DrawWireCube(m_Salt.transform.position, Vector3.one * 0.5f);
        else
            Gizmos.DrawWireSphere(FallEndPosition, m_Radius);

        RaycastHit[] hitsSalt = Physics.SphereCastAll(FallEndPosition, m_Radius, Vector3.up, 0, m_Protactor.GetEnemyLayer());
        for (int i = 0; i < hitsSalt.Length; i++)
        {
            if (hitsSalt[i].collider.TryGetComponent<EnemyController>(out EnemyController enemyController))
            {
                Gizmos.color = Color.white;
                Gizmos.DrawWireSphere(hitsSalt[i].transform.position, 1);
                Debug.Log(hitsSalt[i].collider.name);
            }
        }
    }
}