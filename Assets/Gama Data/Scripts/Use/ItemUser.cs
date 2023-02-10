using UnityEngine;
using Zenject;

[RequireComponent(typeof(Protactor))]
public class ItemUser : MonoBehaviour
{
    [Inject] private readonly InputHandler m_InputHandler;
    [Inject] private readonly Inventory m_Inventory;

    [Inject] private readonly HeadRoatator m_HeadRoatator;

    public static ItemUser Instance { get; private set; }

    public Protactor Protactor { get; private set; }

    private SlotSelector m_SlotSelector;
    private Transform m_Head;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        m_SlotSelector = m_Inventory.GetSlotSelector();
        Protactor = GetComponent<Protactor>();

        m_Head = m_HeadRoatator.transform;
    }

    private void Update()
    {
        if (m_InputHandler.Use() == true)
            Use();
    }

    public Slot GetSlot()
    {
        int slotIndex = m_SlotSelector.SlotSelectorModel.CurrentSelectionSlotIndex;
        Slot slot = m_Inventory.GetSlots()[slotIndex];

        return slot;
    }

    private void Use()
    {
        Item item = GetSlot().SlotModel.Item;

        if (item == null) return;

        if(item.GetItemType() == ItemType.Protection)
        {
            Protactor.Use(GetSlot());
        }
        else if(item.GetItemType() == ItemType.Quest)
        {

        }
    }
}
