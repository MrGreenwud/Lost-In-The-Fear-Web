using UnityEngine;
using System.Collections.Generic;

public class Storage : MonoBehaviour
{
    [SerializeField] private List<Slot> m_Slots;
    [SerializeField] private SlotSelector m_SlotSelector;

    public List<Slot> GetSlots() => m_Slots;
    public SlotSelector GetSlotSelector() => m_SlotSelector;

    protected void Init()
    {
        m_SlotSelector.Init(this);

        for (int i = 0; i < m_Slots.Count; i++)
            m_Slots[i].Init(i, m_SlotSelector);
    }

    public virtual GameObject CreateObject(GameObject gameObject, Vector3 position)
    {
        return Instantiate(gameObject, position, Quaternion.identity);
    }
}
