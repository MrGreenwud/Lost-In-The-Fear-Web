public class DiaryView : StorageView
{
    private Diary m_Diary;

    public DiaryView(Diary diary) : base(diary) 
    { 
        m_Diary = diary;
    }

    public override void UpdateViewAllSlots()
    {
        base.UpdateViewAllSlots();
    }
}
