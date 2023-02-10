using System;
using UnityEngine;

public class DiaryModel : StorageModel
{
    private int m_CurrentPage = 1;
    private int m_PageCount;

    private Diary m_Diary;

    public int GetCurrentPage() => m_CurrentPage;

    public void ScrollForword()
    {
        SetCurrentPage(m_CurrentPage + 1);
    }

    public void ScrollBack()
    {
        SetCurrentPage(m_CurrentPage - 1);
    }

    private void SetCurrentPage(int page)
    {
        if (page > m_PageCount) return;
        else if (page < 1) return;
        else m_CurrentPage = page;

        UpdateSlotsStorage();
    }

    public DiaryModel(Diary diary) : base(diary)
    {
        m_Diary = diary;
    }

    public override void AddItem(Item newItem)
    {
        if (newItem == null) return;

        m_Diary.GetSlotsModel().Add(new SlotModel(newItem));
        m_PageCount = (int)Math.Ceiling((double)m_Diary.GetSlotsModel().Count / (double)6);
        
        base.AddItem(newItem);
    }

    public override void RemoveItemByItem(Item item) { }

    public void PikUpItem(GameObjectNote note)
    {
        if (note == null)
        {
            BCTSTool.BCTSDebug.LogError("GameObjectItem is null");
            return;
        }

        if (note.GetNote() == null)
        {
            BCTSTool.BCTSDebug.LogError("This GameObjectItem is dosn't have item");
            return;
        }

        AddItem(note.GetNote());
        note.DestroyObject();
    }

    public override void DropItem(Slot slot, Vector3 position) { }

    private void UpdateSlotsStorage()
    {
        int a = 0;
        for (int i = m_CurrentPage * 6 - 6; a < p_Storage.GetSlots().Count; i++, a++)
        {
            if(i < m_Diary.GetSlotsModel().Count)
                p_Storage.GetSlots()[a].SlotModel.SetItem(m_Diary.GetSlotsModel()[i].Item);
            else
                p_Storage.GetSlots()[a].SlotModel.SetItem(null);
        }
    }
}
