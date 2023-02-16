using UnityEngine;

public class ParticleProtection : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_ParticleSystem;
    [SerializeField] private Protactor m_Protactor;

    [Space(10)]

    [SerializeField] private Color m_HollyWaterColor;
    [SerializeField] private Color m_SaltColor;

    private void OnEnable()
    {
        m_Protactor.OnDrop += Play;
    }

    private void OnDisable()
    {
        m_Protactor.OnDrop -= Play;
    }

    public void Play(Item item)
    {
        if(item.GetID() == 1)
            m_ParticleSystem.startColor = m_HollyWaterColor;
        else
            m_ParticleSystem.startColor = m_SaltColor;

        m_ParticleSystem.Play();
    }
}
