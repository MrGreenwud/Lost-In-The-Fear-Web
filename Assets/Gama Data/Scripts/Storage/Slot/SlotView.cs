using Unity.Profiling;
using UnityEngine;

public class SlotView
{
    private Slot m_Slot;

    private ProfilerMarker UpdateSlotView = new ProfilerMarker(ProfilerCategory.Video, "Update Slot View");
    //Inventory (AddItems);

    public SlotView(Slot slot)
    {
        m_Slot = slot;
        UpdateView();
    }

    public void UpdateView()
    {
        UpdateSlotView.Begin();

        Item item = m_Slot.SlotModel.Item;

        if (item != null)
        {
            m_Slot.GetImage().sprite = item.GetIcon();
            m_Slot.GetImage().enabled = true;
            
            if (m_Slot.GetTextMeshPro() != null)
            {
                m_Slot.GetTextMeshPro().text = item.GetItemName();
                m_Slot.GetTextMeshPro().enabled = true;
            }

            if (item.GetIsStacable() == false)
            {
                if (m_Slot.GetUseCountBar() != null)
                {
                    if (item.GetMaxStecSize() > 1)
                    {
                        m_Slot.GetUseCountBar().fillAmount = (float)m_Slot.SlotModel.Count / item.GetMaxStecSize();
                        m_Slot.GetUseCountBar().enabled = true;
                    }
                }
            }
        }
        else
        {

            m_Slot.GetImage().sprite = null;
            m_Slot.GetImage().enabled = false;

            if (m_Slot.GetTextMeshPro() != null)
                m_Slot.GetTextMeshPro().enabled = false;

            if (m_Slot.GetUseCountBar() != null)
                m_Slot.GetUseCountBar().enabled = false;
        }

        UpdateSlotView.End();
    }
}
