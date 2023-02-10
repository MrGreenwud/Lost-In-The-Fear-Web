using UnityEngine;
using Zenject;

public class SearchHelper : MonoBehaviour
{
    [Inject] private readonly PlayerController m_PlayerController;
    
    [SerializeField] private float m_SearchDistence = 5;
    [SerializeField] private LayerMask m_SearchLayer;

    private Transform m_FindedObject;
    private float m_MinDistence;

    [Space(10)]

    [SerializeField] private Transform m_Commpas;
    [SerializeField] private SpriteRenderer m_CommpasSpriteRenderer;
    
    private Color m_ArrowOpecityColor;
    
    private void Awake()
    {
        m_ArrowOpecityColor = m_CommpasSpriteRenderer.color;
    }

    private void Update()
    {
        Search();
        SetCommpasRotation();
        SetOpacityByDistence();
    }

    private void Search()
    {
        if (m_PlayerController.IsMove == false) return;

        RaycastHit[] hits = Physics.SphereCastAll(m_PlayerController.transform.position, m_SearchDistence, Vector3.up, 0, m_SearchLayer);

        if (hits.Length == 0)
        {
            m_FindedObject = null;
            return;
        }

        m_MinDistence = m_SearchDistence + 10;

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.TryGetComponent<GameObjectItem>(out GameObjectItem gameObjectItem))
            {
                if (gameObjectItem.GetItem().GetItemType() != ItemType.Quest) continue;

                SetObejectByMinDistence(hits[i].transform);
            }
            else if (hits[i].collider.TryGetComponent<QuestCompliter>(out QuestCompliter questCompliter))
            {
                if (questCompliter.IsComplite == true) continue;

                SetObejectByMinDistence(hits[i].transform);
            }
        }
    }

    public Vector3 GetDirectionToItem()
    {
        if (m_FindedObject == null) return Vector3.zero;
        return (m_FindedObject.position - m_PlayerController.transform.position).normalized;
    }

    private void SetCommpasRotation()
    {
        if (m_FindedObject == null) return;

        Vector3 diration = GetDirectionToItem();
        diration.y = 0;

        m_Commpas.transform.rotation = Quaternion.LookRotation(diration);
    }

    private void SetOpacityByDistence()
    {
        if(m_FindedObject == null)
        {
            m_ArrowOpecityColor.a = 0;
            m_CommpasSpriteRenderer.color = m_ArrowOpecityColor;
            return;
        }

        float distence = Vector3.Distance(m_FindedObject.position, m_PlayerController.transform.position);
        
        if(distence > m_SearchDistence)
        {
            m_ArrowOpecityColor.a = 0;
            m_CommpasSpriteRenderer.color = m_ArrowOpecityColor;
            return;
        }

        float opacity = 1 - distence / m_SearchDistence;
        m_ArrowOpecityColor.a = opacity * 2;
        m_CommpasSpriteRenderer.color = m_ArrowOpecityColor;
    }

    private void SetObejectByMinDistence(Transform newObejct)
    {
        float newDistence = Vector3.Distance(m_PlayerController.transform.position, newObejct.position);

        if (newDistence < m_MinDistence)
        {
            m_MinDistence = newDistence;
            m_FindedObject = newObejct.transform;
        }
    }

    public void OnDrawGizmos()
    {
        if (Application.IsPlaying(this) == false) return; 

        Vector3 diraction = GetDirectionToItem();
        diraction.y = 0;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(m_PlayerController.transform.position, GetDirectionToItem() * 10);
        Gizmos.DrawWireSphere(m_PlayerController.transform.position, m_SearchDistence);
    }
}
