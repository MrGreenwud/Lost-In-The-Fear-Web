using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using SelectorType = SlotSelector.SelectorType;

[RequireComponent(typeof(EventTrigger))]
public class Slot : MonoBehaviour
{
    public SlotModel SlotModel { get; private set; }
    public SlotView SlotView { get; private set; }

    [SerializeField] private Image m_Image;
    [SerializeField] private TextMeshProUGUI m_TextMeshProUGUI;

    [SerializeField] private Image m_UseCountBar;

    private SlotSelector m_SlotSelector;
    
    private int m_Index;

    public Image GetImage() => m_Image;
    public TextMeshProUGUI GetTextMeshPro() => m_TextMeshProUGUI;

    public Image GetUseCountBar() => m_UseCountBar;

    public void Init(int index, SlotSelector slotSelector)
    {
        SlotModel = new SlotModel(null);
        SlotView = new SlotView(this);

        m_Index = index;
        m_SlotSelector = slotSelector;

        SlotModel.OnUpdateView += SlotView.UpdateView;
    }

    private void OnDestroy()
    {
        SlotModel.OnUpdateView -= SlotView.UpdateView;
    }

    public void Select()
    {
        if(m_SlotSelector.GetSelectorType() == SelectorType.Clickerd || m_SlotSelector.GetSelectorType() == SelectorType.Both)
            m_SlotSelector.SlotSelectorModel.Select(m_Index);
    }
}