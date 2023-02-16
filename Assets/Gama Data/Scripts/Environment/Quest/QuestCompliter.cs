using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using Zenject;

public class QuestCompliter : Interacteble
{
    [Serializable]
    public class Actions
    {
        public UnityEvent OnComplite;
        public bool IsComplite { get; private set; }

        [SerializeField] private Item[] m_RequiredItems;

        [SerializeField] private uint MessageID;

        public Item[] GetRequiredItems()
        {
            Item[] tempRequiredItems = new Item[m_RequiredItems.Length];

            for (int i = 0; i < m_RequiredItems.Length; i++)
                tempRequiredItems[i] = m_RequiredItems[i];

            return tempRequiredItems;
        }

        public uint GetMessageID() => MessageID;

        public void Complite()
        {
            IsComplite = true;
            OnComplite?.Invoke();
        }
    }

    public enum QuestType
    {
        Default,
        One_Main_Action,
        Ñonsistent
    }

    [Inject] private readonly Inventory m_Inventory;

    [SerializeField] private QuestType m_QuestType;

    [ShowIf("m_QuestType", QuestType.One_Main_Action)]
    [SerializeField] protected Actions m_MainAction;

    [SerializeField] protected Actions[] m_Actions;

    public bool IsComplite { get; private set; }
    public Action OnComplite;

    public uint GetMessageID()
    {
        if (m_QuestType == QuestType.One_Main_Action)
        {
            if (m_MainAction.IsComplite == false && Complite(m_MainAction) == false)
                return m_MainAction.GetMessageID();
        }

        for (int i = 0; i < m_Actions.Length; i++)
        {
            if (m_Actions[i].IsComplite == false)
            {
                if (Complite(m_Actions[i]) == true)
                    break;
                
                return m_Actions[0].GetMessageID();
            }
        }

        return 0;
    }

    public override void Interact()
    {
        if (m_MainAction.IsComplite == true || m_QuestType == QuestType.Default)
        {
            for (int a = 0; a < m_Actions.Length; a++)
                if (m_Actions[a].IsComplite == false)
                    if (Complite(m_Actions[a]) == true) break;
        }
        else if (m_QuestType == QuestType.Ñonsistent)
        {
            for (int a = 0; a < m_Actions.Length; a++)
                if (m_Actions[0].IsComplite == false)
                    if (Complite(m_Actions[a]) == false) return;
        }
        else if (m_QuestType == QuestType.One_Main_Action)
        {
            if(m_MainAction.GetRequiredItems().Length == 0) return;
            Complite(m_MainAction);
        }

        if(CheckAllActionByComplite() == true)
        {
            IsComplite = true;
            OnComplite?.Invoke();
        }

        Debug.Log($"{(int)m_QuestType} {m_MainAction.IsComplite}");
    }

    private bool Complite(Actions action)
    {
        Item[] tempRequiredItems = action.GetRequiredItems();
        List<Slot> requiredItemsSlots = m_Inventory.InventoryModel.FindAllSlotsByItems(tempRequiredItems);

        if (requiredItemsSlots == null)
            return false;

        if (requiredItemsSlots.Count == tempRequiredItems.Length)
        {
            for (int i = 0; i < requiredItemsSlots.Count; i++)
                requiredItemsSlots[i].SlotModel.UseItem();

            action.Complite();
            return true;
        }

        return false;
    }

    private bool CheckAllActionByComplite()
    {
        if (m_Actions.Length == 0) 
            return true;

        for (int a = 0; a < m_Actions.Length; a++)
        {
            if (m_Actions[0].IsComplite == false)
                return false;
        }

        return true;
    }
}
