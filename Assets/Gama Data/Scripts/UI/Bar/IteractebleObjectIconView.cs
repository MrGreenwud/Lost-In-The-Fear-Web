using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class IteractebleObjectIconView : MonoBehaviour
{
    [Inject] private readonly PlayerController m_PlayerController;

    [SerializeField] private float m_CheckDistence;
    [SerializeField] private LayerMask m_ItemLayers;
    [SerializeField] private Camera m_Camera;

    [Space(10)]

    [SerializeField] private Image[] m_PickUpItemIcon;
    [SerializeField] private Image m_PickUpQuestItemIcon;
    [SerializeField] private Image m_QuestIcon;

    [Space(10)]

    [SerializeField] private Vector3 m_PositionOffset = new Vector3(0, 1, 0);

    private void Update()
    {
        Vector3 spherePosition = m_PlayerController.transform.position;
        RaycastHit[] hits = Physics.SphereCastAll(spherePosition, m_CheckDistence, Vector3.up, 0.001f, m_ItemLayers);

        Queue<Image> pickUpItemIcon = new Queue<Image>();

        for (int i = 0; i < m_PickUpItemIcon.Length; i++)
            pickUpItemIcon.Enqueue(m_PickUpItemIcon[i]);

        bool isQuestItem = false;

        List<QuestCompliter> questCompliters = new List<QuestCompliter>();

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.TryGetComponent<GameObjectItem>(out GameObjectItem gameObjectItem))
            {
                Vector3 ditectionToPlayer = spherePosition - hits[i].transform.position;

                if (-Vector3.Dot(m_Camera.transform.forward, ditectionToPlayer) < 0)
                    continue;

                if (gameObjectItem.GetItem().GetItemType() == ItemType.Quest)
                {
                    m_PickUpQuestItemIcon.enabled = true;
                    Vector3 iconPosition = m_Camera.WorldToScreenPoint(hits[i].transform.position);
                    m_PickUpQuestItemIcon.transform.position = iconPosition;
                    isQuestItem = true;
                }
                else if (pickUpItemIcon.Count > 0)
                {
                    Vector3 iconPosition = m_Camera.WorldToScreenPoint(hits[i].transform.position + m_PositionOffset);
                    Image icon = pickUpItemIcon.Dequeue();

                    icon.enabled = true;
                    icon.transform.position = iconPosition;
                }
            }
            else if(hits[i].collider.TryGetComponent<QuestCompliter>(out QuestCompliter questCompliter))
            {
                questCompliters.Add(questCompliter);
            }
        }

        bool isQuest = false;

        if (questCompliters.Count > 0)
        {
            QuestCompliter nearQuestCompliter = questCompliters[0];
            float distenceToNearQuestCompliter = Vector3.Distance(spherePosition, nearQuestCompliter.transform.position);

            for(int i = 1; i < questCompliters.Count; i++)
            {
                float newDistence = Vector3.Distance(spherePosition, questCompliters[i].transform.position);

                if (newDistence < distenceToNearQuestCompliter)
                {
                    nearQuestCompliter = questCompliters[i];
                    distenceToNearQuestCompliter = newDistence;
                }
            }

            Vector3 ditectionToPlayer = spherePosition - nearQuestCompliter.transform.position;

            if (-Vector3.Dot(m_Camera.transform.forward, ditectionToPlayer) > 0)
            {
                Vector3 iconPosition = m_Camera.WorldToScreenPoint(nearQuestCompliter.transform.position);

                m_QuestIcon.enabled = true;
                m_QuestIcon.transform.position = iconPosition;
                isQuest = true;
            }
        }

        if(pickUpItemIcon.Count > 0)
            pickUpItemIcon.Dequeue().enabled = false;

        m_PickUpQuestItemIcon.enabled = isQuestItem;
        m_QuestIcon.enabled = isQuest;
    }
}