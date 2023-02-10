using UnityEngine;

public class Inventory : Storage
{
    public InventoryModel InventoryModel { get; private set; }
    public InventoryView InventoryView { get; private set; }

    public SlotModel CarrubleItemSlot { get; private set; }

    private void Awake()
    {
        Init();

        InventoryModel = new InventoryModel(this);
        InventoryView = new InventoryView(this);

        CarrubleItemSlot = new SlotModel();

        InventoryModel.OnUpdateStorage += InventoryView.UpdateViewAllSlots;
    }

    private void OnDestroy()
    {
        InventoryModel.OnUpdateStorage -= InventoryView.UpdateViewAllSlots;
    }

    public override GameObject CreateObject(GameObject gameObject, Vector3 position)
    {
        return base.CreateObject(gameObject, position);
    }
}
