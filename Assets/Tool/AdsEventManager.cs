using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Zenject;

public class AdsEventManager : MonoBehaviour
{
    [SerializeField] private PlayerController m_PlayerController;

    public static AdsEventManager s_Instence;
    public UnityEvent AfterDeaths;

    [SerializeField] private uint m_MaxDeathCount = 3;
    public uint DeathCount { get; private set; }

    private void Awake()
    {
        if(s_Instence == null)
            s_Instence = this;
    }

    public void OnEnable()
    {
        m_PlayerController.OnDeath += s_Instence.IncreaseDeathCount;
        m_PlayerController.OnDeath += s_Instence.CheckDeathCount;
    }

    private void OnDisable()
    {
        m_PlayerController.OnDeath -= s_Instence.IncreaseDeathCount;
        m_PlayerController.OnDeath -= s_Instence.CheckDeathCount;
    }

    public void IncreaseDeathCount()
    {
        DeathCount++;
    }

    public void ResetDeathCount()
    {
        DeathCount = 0;
    }

    public void CheckDeathCount()
    {
        if (DeathCount >= m_MaxDeathCount)
        {
            AfterDeaths?.Invoke();
            ResetDeathCount();
        }

        Debug.Log(DeathCount);
    }
}
