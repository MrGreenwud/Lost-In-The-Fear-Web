using System;
using UnityEngine;
using UnityEditor;
using BCTSTool.Math;

public class Protactor : MonoBehaviour
{
    public SaltDroper SaltDroper { get; private set; }
    public HollyWoterDroper HollyWoterDroper { get; private set; }

    public Action<EnemyController, float> OnEnemyStan;
    public Action<Item> OnDrop;

    [Header("Mask Settings")]

    [SerializeField] private LayerMask m_EnemyLayer;
    [SerializeField] private LayerMask m_GroundLayer;
    [SerializeField] private LayerMask m_ColitionDetection;

    [Header("Hally Water Setting")]

    [SerializeField] private float m_HollyWaterUseBoxSize;
    [SerializeField] private float m_HollWaterBoxForworOffset;

    [Space(10)]
    [Header("Salt Setting")]

    [SerializeField] private float m_MaxSaltDropDistence;
    [SerializeField] private float m_FallSpeed;
    
    [Space(5)]

    [SerializeField] private GameObject m_SaltPrafab;
    private GameObject m_Salt;

    [Space(10)]
    [Header("LerpCurve Settings")]

    [SerializeField] private float m_DropAmpletude;

    private Protation m_ProtationType;
    private float m_StanTime;
    private float m_StanDistance;

    public LayerMask GetEnemyLayer() => m_EnemyLayer;
    public LayerMask GetGroundLayer() => m_GroundLayer;
    public LayerMask GetColitionDetection() => m_ColitionDetection;

    public float GetHollyWaterUseBoxSize() => m_HollyWaterUseBoxSize;
    public float GetHollWaterBoxForworOffset() => m_HollWaterBoxForworOffset;

    public float GetMaxSaltDropDistence() => m_MaxSaltDropDistence;
    public float GetFallSpeed() => m_FallSpeed;
    public float GetDropAmpletude() => m_DropAmpletude;
    public GameObject GetSalt() => m_Salt;

    public void SetPotactionType(Protation protationType)
    {
        if (protationType == Protation.Null) return;
        m_ProtationType = protationType;
    }

    public void SetStanTime(float stanTime)
    {
        if (stanTime < 1) return;
        m_StanTime = stanTime;
    }

    public void SetStanDistance(float stanDistence)
    {
        if (stanDistence < 1) return;
        m_StanDistance = stanDistence;
    }

    public void ResetPotactionParametor()
    {
        m_ProtationType = Protation.Null;
        m_StanTime = 0;
        m_StanDistance = 0;
    }

    private void Awake()
    {
        m_Salt = Instantiate(m_SaltPrafab);
        m_Salt.SetActive(false);

        SaltDroper = new SaltDroper(this);
        HollyWoterDroper = new HollyWoterDroper(this);

        SaltDroper.OnFallDown += GEN;
    }

    private void Update()
    {
        SaltDroper.Update();
    }

    public void Use(Slot slot)
    {
        SlotModel slotModel = slot.SlotModel;
        slotModel.Item.Use();

        if (m_ProtationType == Protation.Salt)
        {
            OnDrop?.Invoke(slot.SlotModel.Item);

            SaltDroper.Drop(m_StanTime, m_StanDistance);
            slotModel.UseItem();
        }
        else if (m_ProtationType == Protation.Holly_Water)
        {
            OnDrop?.Invoke(slot.SlotModel.Item);

            HollyWoterDroper.Drop(m_StanTime);
            slotModel.UseItem();
        }
    }

    private void GEN()
    {
        GameObject g = Instantiate(m_SaltPrafab);
        g.transform.position = SaltDroper.FallEndPosition;
    }

    private void OnDrawGizmosSelected()
    {
        //Holly Water
        Vector3 boxSize = Vector3.one * m_HollyWaterUseBoxSize;
        Vector3 prosition = transform.position + transform.forward * m_HollWaterBoxForworOffset;

        RaycastHit[] hits = Physics.BoxCastAll(prosition, boxSize * 0.5f, transform.forward, transform.rotation, 0, m_EnemyLayer);

        Gizmos.color = Color.white;

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.TryGetComponent<EnemyController>(out EnemyController enemyController))
            {
                Gizmos.DrawWireSphere(hits[i].collider.transform.position, 1);
                Debug.Log(hits[i].collider.name);
            }
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(prosition, boxSize);
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        //Salt
        float maxSaltDropDistence;

        RaycastHit wallHit;
        if (Physics.Raycast(transform.position, transform.forward, out wallHit, m_MaxSaltDropDistence, m_ColitionDetection))
        {
            maxSaltDropDistence = Vector3.Distance(transform.position, wallHit.point);

            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, wallHit.point);

            Handles.color = Color.red;
            Handles.DrawWireDisc(wallHit.point, wallHit.normal, 0.5f);
        }
        else
        {
            maxSaltDropDistence = m_MaxSaltDropDistence;
        }

        Vector3 groundChackerPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z) +
        transform.forward * maxSaltDropDistence - transform.forward * 0.2f;

        RaycastHit groundHit;
        if (Physics.Raycast(groundChackerPosition, Vector3.down, out groundHit, 10000, m_GroundLayer))
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(groundChackerPosition, groundHit.point);

            Handles.color = Color.green;
            Handles.DrawWireDisc(groundHit.point, Vector3.up, 0.5f);
        }

        LerpCureve.Draw(transform.position, groundHit.point, transform.position, m_DropAmpletude);
        
        if(Application.isPlaying == true)
        {
            SaltDroper.OnDrawGizmos();
        }
    }

#endif
}