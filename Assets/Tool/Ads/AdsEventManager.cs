using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class AdsEventManager : MonoBehaviour
{
    public static AdsEventManager s_Instence;

    [Inject] private readonly PlayerController m_PlayerController;

    public UnityEvent AfterDeaths;
    [SerializeField] private uint m_MaxDeathCount = 3;

    private void Awake()
    {
        if(s_Instence == null)
            s_Instence = this;
    }

    public void OnEnable()
    {
        m_PlayerController.OnDeath += s_Instence.CheckDeathCount;
        m_PlayerController.OnDeath += PlayerDeathCounter.IncreaseDeathCount;
    }

    private void OnDisable()
    {
        m_PlayerController.OnDeath -= s_Instence.CheckDeathCount;
        m_PlayerController.OnDeath += PlayerDeathCounter.IncreaseDeathCount;
    }

    public void CheckDeathCount()
    {
        if (PlayerDeathCounter.DeathCount >= m_MaxDeathCount)
        {
            AfterDeaths?.Invoke();
            PlayerDeathCounter.ResetDeathCount();
        }

        Debug.Log(PlayerDeathCounter.DeathCount);
    }
}