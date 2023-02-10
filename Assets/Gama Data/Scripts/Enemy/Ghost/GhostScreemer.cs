using System.Collections;
using UnityEngine;

public class GhostScreemer : MonoBehaviour
{
    [SerializeField] private GameObject m_ScreemScene;
    [SerializeField] private float m_Time = 3;

    public void OnScreem()
    {
        m_ScreemScene.SetActive(true);
        StartCoroutine(DesebleScreem(m_Time));
    }

    IEnumerator DesebleScreem(float time)
    {
        yield return new WaitForSeconds(time);
        m_ScreemScene.SetActive(false);
    }
}
