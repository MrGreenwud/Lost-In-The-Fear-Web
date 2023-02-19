using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class AdsEventManager : MonoBehaviour
{
    [Inject] private readonly PlayerController m_PlayerController;

    public UnityEvent AfterDeaths;
    [SerializeField] private uint m_MaxDeathCount = 3;

    [Space(10)]

    public UnityEvent OnRebornButtonClick;
    public UnityEvent OnReborn;

    public void OnEnable()
    {
        m_PlayerController.OnDeath += CheckDeathCount;
        m_PlayerController.OnDeath += PlayerDeathCounter.IncreaseDeathCount;
    }

    private void OnDisable()
    {
        m_PlayerController.OnDeath -= CheckDeathCount;
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

    public void Reborn()
    {
        OnReborn?.Invoke();
    }
}