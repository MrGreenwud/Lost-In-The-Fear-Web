using UnityEngine;
using TMPro;
using Zenject;
using BCTSTool.Localization;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ItemNameText : MonoBehaviour
{
    [Inject] private readonly Inventory m_Inventory;

    private TextMeshProUGUI m_TextMeshProUGUI;

    private float m_AlphaText = 1f;

    private void Awake()
    {
        m_TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
        ChengeAlpha(0);
    }

    private void OnEnable()
    {
        m_Inventory.GetSlotSelector().OnSelect += Show;
    }

    private void OnDisable()
    {
        m_Inventory.GetSlotSelector().OnSelect -= Show;
    }

    private void Update()
    {
        if(m_TextMeshProUGUI.color.a > 0)
            ChengeAlpha(m_AlphaText -= Time.deltaTime * 0.8f);
    }

    public void Show()
    {
        int slotIndex = m_Inventory.GetSlotSelector().SlotSelectorModel.CurrentSelectionSlotIndex;
        Slot slot = m_Inventory.GetSlots()[slotIndex];

        if (slot.SlotModel.Item == null) 
            return;

        m_AlphaText = 1;
        ChengeAlpha(1);

        uint id = slot.SlotModel.Item.GetID();

        m_TextMeshProUGUI.text = Settings.s_Lenguage.GetTextByID(id);
    }

    private void ChengeAlpha(float alpha)
    {
        Color newColor = m_TextMeshProUGUI.color;

        newColor.a = alpha;
        m_TextMeshProUGUI.color = newColor;
    }
}