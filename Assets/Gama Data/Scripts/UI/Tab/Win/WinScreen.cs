using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private Animator[] m_Animator;
    [SerializeField] private QuestMessage m_QuestMessage;
    [SerializeField] private GhostEnemyController m_GhostEnemyController;
    [SerializeField] private GhostEnemySpawner m_GhostEnemySpawner;

    private void Awake()
    {
        Debug.LogError(gameObject.name);
    }

    public void Win()
    {
        m_GhostEnemyController.ApplyStan(2000f);
        m_GhostEnemySpawner.OnStan(2000f);

        for (int i = 0; i < m_Animator.Length; i++)
            m_Animator[i].SetBool("isOpen", false);

        StartCoroutine(Show());
    }

    private IEnumerator Show()
    {
        yield return new WaitForSeconds(3);
        m_QuestMessage.Show(99991, 0.5f);
        yield return new WaitForSeconds(5);
        m_QuestMessage.Show(99992, 0.5f);
        yield return new WaitForSeconds(5);
        m_QuestMessage.Show(99993, 0.5f);
        yield return new WaitForSeconds(5);
        m_QuestMessage.Show(99994, 1);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
    }
}
