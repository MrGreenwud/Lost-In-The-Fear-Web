using UnityEngine;
using System.Collections.Generic;

public class Diary : Storage
{
    public DiaryModel DiaryModel { get; private set; }
    public DiaryView DiaryView { get; private set; }

    private List<SlotModel> m_SlotsModel = new List<SlotModel>();

    public List<SlotModel> GetSlotsModel() => m_SlotsModel;

    private void Awake()
    {
        Init();

        DiaryModel = new DiaryModel(this);
        DiaryView = new DiaryView(this);

        DiaryView.UpdateViewAllSlots();
    }
}
