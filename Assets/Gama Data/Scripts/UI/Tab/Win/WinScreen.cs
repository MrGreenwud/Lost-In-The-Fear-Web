using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private Animator[] m_Animator;
    [SerializeField] private QuestMessage m_QuestMessage;

    public void Win()
    {
        for(int i = 0; i < m_Animator.Length; i++)
            m_Animator[i].SetBool("isOpen", false);

        StartCoroutine(Show());
    }

    private IEnumerator Show()
    {
        yield return new WaitForSeconds(3);
        m_QuestMessage.Show(9999);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
    }
}
