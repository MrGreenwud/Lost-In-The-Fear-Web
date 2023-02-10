using Unity.Profiling;
using UnityEngine;

public class StorageView
{
    protected Storage p_Storage;

    private ProfilerMarker UpdateStorageView = new ProfilerMarker(ProfilerCategory.Video, "Update All Slot View");

    public StorageView(Storage storage)
    {
        p_Storage = storage;
        UpdateViewAllSlots();
    }

    public virtual void UpdateViewAllSlots()
    {
        UpdateStorageView.Begin();

        for (int i = 0; i < p_Storage.GetSlots().Count; i++)
            p_Storage.GetSlots()[i].SlotView.UpdateView();

        UpdateStorageView.End();
    }
}
