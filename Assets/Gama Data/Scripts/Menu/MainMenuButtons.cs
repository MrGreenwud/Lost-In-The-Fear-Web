using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public enum Tab
    {
        MainMenu,
        Settings
    }

    [SerializeField] private GameObject[] m_Settings;
    [SerializeField] private GameObject[] m_MainMenu;

    private void Awake()
    {
        SwichTab(Tab.MainMenu);
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Settings()
    {
        SwichTab(Tab.Settings);
    }

    public void Back()
    {
        SwichTab(Tab.MainMenu);
    }

    public void SwichTab(Tab tab)
    {
        bool settings = false;
        bool mainMenu = false;

        if (tab == Tab.MainMenu)
            mainMenu = true;
        else
            settings = true;

        for (int i = 0; i < m_Settings.Length; i++)
            m_Settings[i].SetActive(settings);

        for (int i = 0; i < m_MainMenu.Length; i++)
            m_MainMenu[i].SetActive(mainMenu);
    }
}
