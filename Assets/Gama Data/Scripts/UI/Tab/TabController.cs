using UnityEngine;
using Zenject;
using UnityEngine.SceneManagement;

public class TabController : MonoBehaviour
{
    [Inject] private InputHandler m_InputHundler;
    [Inject] private PlayerController m_PlayerController;
    [Inject] private HeadRoatator m_HeadRoatator;

    [SerializeField] private DiaryTab m_Diary;
    [SerializeField] private ReadTab m_ReadTab;
    [SerializeField] private DeathTab m_DeathTab;

    public Tab CurrentTab { get; private set; }

    public ReadTab GetReadTab() => m_ReadTab;

    private void Awake()
    {
        m_PlayerController.OnDeath += ShowDeathTab;
    }

    private void Update()
    {
        if (m_InputHundler.GetNoteList() == true)
            SwitchTab(m_Diary);
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
    
    public void ShowDeathTab()
    {
        SwitchTab(m_DeathTab);
    }

    public void OpenReadTab()
    {
        SwitchTab(m_ReadTab);
    }

    public void CloseTab()
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
