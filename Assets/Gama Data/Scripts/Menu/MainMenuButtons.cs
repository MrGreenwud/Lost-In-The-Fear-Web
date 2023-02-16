using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))] 
public class MainMenuButtons : MonoBehaviour
{
    public enum Tab
    {
        MainMenu,
        Settings
    }

    [SerializeField] private GameObject[] m_Settings;
    [SerializeField] private GameObject[] m_MainMenu;

    [Space(10)]

    [SerializeField] private Animator[] m_Animator;

    [Space(10)]

    private AudioSource m_AudioSource;
    [SerializeField] private AudioClip m_PlayClip;
    [SerializeField] private AudioClip m_ButtonClick;

    private void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();

        SwichTab(Tab.MainMenu);

        CursorState.Show();
        CursorState.UnLock();
    }

    public void Play()
    {
        m_AudioSource.PlayOneShot(m_PlayClip, 0.5f);
        StartCoroutine(OnPlay());
    }

    private IEnumerator OnPlay()
    {
        for (int i = 0; i < m_Animator.Length; i++)
            m_Animator[i].SetBool("isOpen", false);

        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(1);
    }

    public void Settings()
    {
        m_AudioSource.PlayOneShot(m_ButtonClick, 1f);
        SwichTab(Tab.Settings);
    }

    public void Back()
    {
        m_AudioSource.PlayOneShot(m_ButtonClick, 1f);
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
