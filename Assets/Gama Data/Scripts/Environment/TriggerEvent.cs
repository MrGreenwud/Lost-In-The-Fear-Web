using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

[RequireComponent(typeof(Collider))]
public class TriggerEvent : MonoBehaviour
{
    public enum EventType
    {
        All,
        Eneter,
        Stay,
        Exit
    }

    [SerializeField] private UnityEvent m_OnTriggerEnter;
    [SerializeField] private UnityEvent m_OnTriggerStay;
    [SerializeField] private UnityEvent m_OnTriggerExit;

    [SerializeField] private bool m_IsDestroyAffter;

    [ShowIf("m_IsDestroyAffter")]
    [SerializeField] private EventType m_EventType;

    private Collider m_Collider;

    private void Awake()
    {
        m_Collider = GetComponent<Collider>();
        m_Collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        m_OnTriggerEnter.Invoke();

        if(m_EventType == EventType.Eneter || m_EventType == EventType.All)
            if (m_IsDestroyAffter == true)
                Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        m_OnTriggerStay.Invoke();

        if (m_EventType == EventType.Stay || m_EventType == EventType.All)
            if (m_IsDestroyAffter == true)
                Destroy(gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        m_OnTriggerExit.Invoke();

        if (m_EventType == EventType.Exit || m_EventType == EventType.All)
            if (m_IsDestroyAffter == true)
                Destroy(gameObject);
    }
}
