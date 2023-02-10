using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Profiling;

public class StorageModel
{
    public Action OnUpdateStorage;

    protected Storage p_Storage;

    private static ProfilerMarker AddItems = new ProfilerMarker(ProfilerCategory.Scripts, "Inventory (AddItems)");

    public StorageModel(Storage storage)
    {
        p_Storage = storage;
    }

    public virtual void AddItem(Item newItem)
    {
        for (int i = 0; i < p_Storage.GetSlots().Count; i++)
        {
            if (p_Storage.GetSlots()[i].SlotModel.Item == null)
            {
                p_Storage.GetSlots()[i].SlotModel.SetItem(newItem);
                OnUpdateStorage?.Invoke();
                return;
            }
        }
    }

    public virtual void AddItem(Item newItem, Subject subject)
    {
        AddItems.Begin();

        if (newItem.GetIsStacable() == true)
        {
            for (int i = 0; i < p_Storage.GetSlots().Count; i++)
            {
                SlotModel slotModel = p_Storage.GetSlots()[i].SlotModel;

                if (slotModel.Item == newItem)
                {
                    if(slotModel.Count < slotModel.Item.GetMaxStecSize())
                    {
                        int newSlotItemCount = slotModel.Item.GetMaxStecSize() - slotModel.Count;
                        
                        if(newSlotItemCount >= 0)
                            slotModel.AddItemCount(newSlotItemCount);
                        else
                            slotModel.AddItemCount(subject.GetCount());

                        subject.PickUp(newSlotItemCount);

                        if (subject == null)
                        {
                            AddItems.End();
                            OnUpdateStorage?.Invoke();
                            return;
                        }
                    }
                }
            }
        }

        for (int i = 0; i < p_Storage.GetSlots().Count; i++)
        {
            SlotModel slotModel = p_Storage.GetSlots()[i].SlotModel;

            if (slotModel.Item == null)
            {
                int newSlotItemCount = subject.GetCount() - newItem.GetMaxStecSize();

                if (newSlotItemCount == 0)
                {
                    slotModel.SetItem(newItem);
                    subject.PickUp(subject.GetCount());
                }
                else if (newSlotItemCount < 0)
                {
                    slotModel.SetItem(newItem, subject.GetCount());
                    subject.PickUp(subject.GetCount());
                }
                else
                {
                    slotModel.SetItem(newItem);
                    subject.PickUp(newItem.GetMaxStecSize());
                }

                if (subject.GetCount() <= 0)
                {
                    AddItems.End();
                    OnUpdateStorage?.Invoke();
                    return;
                }
            }
        }

        AddItems.End();

        OnUpdateStorage?.Invoke();
    }

    public virtual void RemoveItemByItem(Item item)
    {
        for (int i = 0; i < p_Storage.GetSlots().Count; i++)
        {
            if (p_Storage.GetSlots()[i].SlotModel.Item == item)
            {
                p_Storage.GetSlots()[i].SlotModel.RemoveItem();
                OnUpdateStorage?.Invoke();
                return;
            }
        }

        OnUpdateStorage?.Invoke();
    }

    public Slot FindSlotByItem(Item item)
    {
        if (item == null) return null;

        for(int i = 0; i < p_Storage.GetSlots().Count; i++)
        {
            if (p_Storage.GetSlots()[i].SlotModel.Item == item)
                return p_Storage.GetSlots()[i];
        }

        return null;
    }

    public List<Slot> FindAllSlotsByItems(Item[] items)
    {
        if (items == null) return null;

        List<Slot> slots = new List<Slot>();

        for (int i = 0; i < items.Length; i++)
        {
            for (int a = 0; a < p_Storage.GetSlots().Count; a++)
            {
                if (p_Storage.GetSlots()[a].SlotModel.Item == null)
                    continue;

                if (p_Storage.GetSlots()[a].SlotModel.Item == items[i])
                {
                    bool isDublicate = false;

                    for (int j = 0; j < slots.Count; j++)
                    {
                        if (p_Storage.GetSlots()[a] == slots[j])
                        {
                            isDublicate = true;
                            break;
                        }
                    }

                    if (isDublicate == false)
                    {
                        slots.Add(p_Storage.GetSlots()[a]);
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }

        if (slots.Count > 0)
            return slots;
        else
            return null;
    }

    public virtual void PikUpItem(Subject subject)
    {
        if (subject == null)
        {
            BCTSTool.BCTSDebug.LogError("GameObjectItem is null");
            return;
        }

        if (subject.GetItem() == null)
        {
            BCTSTool.BCTSDebug.LogError("This GameObjectItem is dosn't have item");
            return;
        }

        if (subject.GetItem().GetIsStacable() == false)
        {
            if (CheckFreeSlots() == false) return;
            AddItem(subject.GetItem(), subject);
        }
        else
        {
            AddItem(subject.GetItem(), subject);
        }
    }

    public virtual void DropItem(Slot slot, Vector3 position)
    {
        if (slot == null)
        {
            BCTSTool.BCTSDebug.LogError("Slot is null! " + ToString());
            return;
        }

        if (slot.SlotModel.Item == null)
        {
            BCTSTool.BCTSDebug.LogError("This slot is dosn't have item! " + ToString());
            return;
        }

        if (slot.SlotModel.Item.GetPrefab() == null)
        {
            BCTSTool.BCTSDebug.LogError("This item is dosn't have GameObejectItem Prefab! " + ToString());
            return;
        }

        p_Storage.CreateObject(slot.SlotModel.Item.GetPrefab(), position).GetComponent<Subject>().SetCount(slot.SlotModel.Count);
        RemoveItemByItem(slot.SlotModel.Item);
    }

    protected virtual bool CheckFreeSlots()
    {
        for (int i = 0; i < p_Storage.GetSlots().Count; i++)
        {
            if (p_Storage.GetSlots()[i].SlotModel.Item == null)
                return true;
        }

        return false;
    }
}
