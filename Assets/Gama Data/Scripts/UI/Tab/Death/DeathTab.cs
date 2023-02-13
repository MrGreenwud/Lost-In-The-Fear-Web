using System.Collections;
using Unity.Burst.CompilerServices;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathTab : Tab
{
    [SerializeField] private AudioListener m_AudioListener;
    [SerializeField] private GhostEnemySpawner m_GhostEnemySpawner;
    [SerializeField] private PlayerController m_PlayerController;
    [SerializeField] private TabController m_TabController;

    private void Awake()
    {
        p_View = new DeathTabView(this);
    }

    public override void Open()
    {
        base.Open();
        StartCoroutine(Pouse());
    }

    IEnumerator Pouse()
    {
        yield return new WaitForSeconds(5);

        Time.timeScale = 0;
        m_AudioListener.enabled = false;
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        StartCoroutine(Death());
    }

    public void Reborn()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            m_GhostEnemySpawner.OnStan(30);
            m_PlayerController.PlayerStats.Treat(2);
            m_AudioListener.enabled = true;
            m_TabController.ShowDeathTab();
        }
    }

    public void BackToManinMenu()
    {

    }
}
