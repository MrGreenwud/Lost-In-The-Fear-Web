using UnityEngine;

public class InteractObjectAudio : MonoBehaviour
{
    [SerializeField] private AudioSource m_AudioSource;
    [SerializeField] private AudioClip m_AudioClip;

    public void Play()
    {
        m_AudioSource.transform.position = transform.position;
        m_AudioSource.PlayOneShot(m_AudioClip);
    }
}
