using UnityEngine;

public class InventoryModel : StorageModel
{
    private Inventory m_Inventory;

    public InventoryModel(Storage storage) : base(storage) 
    {
        m_Inventory = storage as Inventory;
    }

    public override void AddItem(Item newItem)
    {
        base.AddItem(newItem);
    }

    public override void RemoveItemByItem(Item item)
    {
        base.RemoveItemByItem(item);
    }

    public virtual void PikUpItem(GameObjectItem gameObjectItem)
    {
        if(gameObjectItem is CarrableGameObject carrableGameObject)
        {
            if (m_Inventory.CarrubleItemSlot.Item != null) return;
            m_Inventory.CarrubleItemSlot.SetItem(carrableGameObject.GetItem());
            carrableGameObject.PickUp();
            return;
        }

        base.PikUpItem(gameObjectItem);
    }

    public override void DropItem(Slot slot, Vector3 position)
    {
        base.DropItem(slot, position);
    }
}
