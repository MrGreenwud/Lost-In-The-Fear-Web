using UnityEngine;

public class SlotSelectorView
{
    private SlotSelector m_SlotSelector;
    private Vector3 m_SelectorTransform;

    public SlotSelectorView(SlotSelector slotSelector)
    {
        m_SlotSelector = slotSelector;
        SelectorChangePosition();
    }

    public void SelectorChangePosition()
    {
        if (m_SlotSelector.Storage.GetSlots().Count == 0) return;

        for(int i = 0; i < m_SlotSelector.SlotSelectorModel.CurrentSelectionSlotIndex + 1; i++)
            m_SelectorTransform = m_SlotSelector.Storage.GetSlots()[i].transform.position;
    }

    public void MoveSelector()
    {
        m_SlotSelector.transform.position = Vector3.Lerp(m_SlotSelector.transform.position, m_SelectorTransform, 10 * Time.deltaTime);
    }
}
