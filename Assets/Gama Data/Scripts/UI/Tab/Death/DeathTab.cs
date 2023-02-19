using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class DeathTab : Tab
{
    [Inject] private readonly AdsEventManager m_AdsEventManager;

    [SerializeField] private AudioListener m_AudioListener;
    [SerializeField] private GhostEnemySpawner m_GhostEnemySpawner;
    [SerializeField] private PlayerController m_PlayerController;
    [SerializeField] private TabController m_TabController;

    [Space(10)]

    [SerializeField] private Animator[] m_Buttons;
    [SerializeField] private Animator[] m_DarckScreen;

    private void Awake()
    {
        p_View = new DeathTabView(this);
    }

    public override void Open()
    {
        base.Open();

        for (int i = 0; i < m_Buttons.Length; i++)
            m_Buttons[i].speed = 1000;

        for (int i = 0; i < m_Buttons.Length; i++)
            m_Buttons[i].SetBool("isOpen", true);

        for (int i = 0; i < m_Buttons.Length; i++)
            m_Buttons[i].speed = 0;

        for (int i = 0; i < m_Buttons.Length; i++)
            m_Buttons[i].SetBool("isOpen", false);

        for (int i = 0; i < m_DarckScreen.Length; i++)
            m_DarckScreen[i].speed = 20;

        for (int i = 0; i < m_DarckScreen.Length; i++)
            m_DarckScreen[i].SetBool("isOpen", true);

        for (int i = 0; i < m_DarckScreen.Length; i++)
            m_DarckScreen[i].speed = 1;

        StartCoroutine(Pouse());
    }

    IEnumerator Pouse()
    {
        yield return new WaitForSeconds(3);

        for (int i = 0; i < m_Buttons.Length; i++)
            m_Buttons[i].speed = 1;

        yield return new WaitForSeconds(3);
        
        Time.timeScale = 0;
    }

    public void Restart()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            StartCoroutine(Death());
        }
    }

    IEnumerator Death()
    {
        for (int i = 0; i < m_DarckScreen.Length; i++)
            m_DarckScreen[i].SetBool("isOpen", false);

        yield return new WaitForSeconds(3);
        
        SceneManager.LoadScene(1);
    }

    public void RebornButtonClick()
    {
        m_AdsEventManager.OnRebornButtonClick?.Invoke();

        Debug.Log("Reborn");
    }

    public void Reborn()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;

            m_PlayerController.PlayerStats.Treat(2);
            m_TabController.ShowDeathTab();
            m_GhostEnemySpawner.OnStan(20);
        }
    }

    public void BackToManinMenu()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }
    }
}
