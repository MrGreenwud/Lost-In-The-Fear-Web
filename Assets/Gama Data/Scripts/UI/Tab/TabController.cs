using UnityEngine;
using Zenject;

public class TabController : MonoBehaviour
{
    [Inject] private InputHandler m_InputHundler;
    [Inject] private PlayerController m_PlayerController;
    [Inject] private HeadRoatator m_HeadRoatator;

    [SerializeField] private DiaryTab m_Diary;
    [SerializeField] private ReadTab m_ReadTab;

    public Tab CurrentTab { get; private set; }

    public ReadTab GetReadTab() => m_ReadTab;

    private void Update()
    {
        if (m_InputHundler.GetNoteList() == true)
            OpenDiaryTab();
    }

    private void SwitchTab(Tab newTab)
    {
        if(CurrentTab != newTab)
        {
            if(CurrentTab == null)
            {
                if (newTab == null) return;

                CurrentTab = newTab;
                CurrentTab.Open();
                OpenTab();
            }
            else
            {
                if (newTab == null)
                {
                    CloseTab();
                }
                else
                {
                    CurrentTab.Close();
                    CurrentTab = newTab;
                    CurrentTab.Open();
                    OpenTab();
                }
            }
        }
        else
        {
            CloseTab();
        }
    }

    public void OpenDiaryTab()
    {
        SwitchTab(m_Diary);
    }
    
    public void OpenReadTab()
    {
        SwitchTab(m_ReadTab);
    }

    private void CloseTab()
    {
        if (CurrentTab == null) return;

        CurrentTab.Close();
        CurrentTab = null;

        CursorState.Hide();
        CursorState.Lock();
        m_PlayerController.UnFreez();
        m_HeadRoatator.UnLock();
    }

    private void OpenTab()
    {
        CursorState.Show();
        CursorState.UnLock();
        m_PlayerController.Freez();
        m_HeadRoatator.Lock();
    }
}
