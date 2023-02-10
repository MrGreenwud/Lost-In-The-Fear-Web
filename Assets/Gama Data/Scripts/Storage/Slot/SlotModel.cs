using System;

public class SlotModel
{
    public Action<Item> OnUpdateItem;
    public Action OnUpdateView;

    public Item Item { get; protected set; }
    public int Count { get; private set; }

    public SlotModel(Item item)
    {
        SetItem(item);
    }

    public SlotModel() { }

    public virtual void SetItem(Item newItem)
    {
        if (newItem == null) return;

        Item = newItem;
        Count = newItem.GetMaxStecSize();

        OnUpdateItem?.Invoke(Item);
    }

    public virtual void SetItem(Item newItem, int count)
    {
        if (newItem == null) return;
        Item = newItem;

        if (count <= newItem.GetMaxStecSize())
            Count = count;
        else
            Count = newItem.GetMaxStecSize();
        
        OnUpdateItem?.Invoke(Item);
    }

    public void RemoveItem()
    {
        if (Item == null) return;
        Item = null;

        OnUpdateItem?.Invoke(Item);
        OnUpdateView?.Invoke();
    }

    public void AddItemCount(int count)
    {
        if (Item == null) return;
        if (Item.GetIsStacable() == false) return;
        if (Count + count > Item.GetMaxStecSize()) return;

        Count += count;
        OnUpdateItem?.Invoke(Item);
    }

    public void UseItem()
    {
        if (Item == null) return;
        if (Count <= 0) return;

        Count--;

        if(Count <= 0)
            Item = null;

        OnUpdateItem?.Invoke(Item);
        OnUpdateView?.Invoke();
    }
}