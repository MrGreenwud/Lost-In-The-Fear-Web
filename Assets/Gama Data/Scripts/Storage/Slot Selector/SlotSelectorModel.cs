using UnityEngine;
using SelectorType = SlotSelector.SelectorType;

public class SlotSelectorModel
{
    public int CurrentSelectionSlotIndex { get; private set; }

    private SlotSelector m_SlotSelector;
    private SelectorType m_SelectorType;

    public void SetCurrentSelectionSlotIndex(int index)
    {
        if (index > m_SlotSelector.Storage.GetSlots().Count - 1)
            CurrentSelectionSlotIndex = 0;
        else if (index < 0)
            CurrentSelectionSlotIndex = m_SlotSelector.Storage.GetSlots().Count - 1;
        else
            CurrentSelectionSlotIndex = index;
    }

    public SlotSelectorModel(SlotSelector slotSelector)
    {
        m_SlotSelector = slotSelector;
        m_SelectorType = m_SlotSelector.GetSelectorType();
    }

    public void Select(int slotIndex)
    {
        SetCurrentSelectionSlotIndex(slotIndex);
        m_SlotSelector.SlotSelectorView.SelectorChangePosition();
    }

    public void Update()
    {
        if (m_SelectorType == SelectorType.Both || m_SelectorType == SelectorType.Scroll_Mouse)
        {
            if (m_SlotSelector.InputHandler.ScrollUp())
                Select(CurrentSelectionSlotIndex + 1);

            if (m_SlotSelector.InputHandler.ScrollDown())
                Select(CurrentSelectionSlotIndex - 1);
        }
    }
}
