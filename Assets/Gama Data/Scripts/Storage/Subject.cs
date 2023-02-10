using UnityEngine;

public class Subject : MonoBehaviour
{
    [SerializeField] private Item m_Item;
    [SerializeField] [Range(1, 100)] private int m_Count = 1;

    [Space(10)]

    [SerializeField] private AudioClip m_Clip;
    [SerializeField] private AudioSource m_Source;

    private ItemSpawnPoint m_ItemSpawnPoint;

    public Item GetItem() => m_Item;
    
    public int GetCount() => m_Count;
    
    public virtual void SetCount(int count)
    {
        if (count <= 0) return;
        m_Count = count;
    }

    private void Awake()
    {
    }

    public void SetItemSpawnPoint(ItemSpawnPoint newItemSpawnPoint)
    {
        m_ItemSpawnPoint = newItemSpawnPoint;
    }

    public void SetAudioSource(AudioSource audioSource)
    {
        m_Source = audioSource;
    }

    public virtual void PickUp(int count = 1)
    {
        if (m_Count >= count)
            m_Count -= count;

        if (m_Clip != null)
        {
            m_Source.transform.position = transform.position;
            m_Source.PlayOneShot(m_Clip);
        }

        if(m_Count == 0)
            DestroyObject();
    }

    public virtual void DestroyObject()
    {
        if (m_Count > 0) return;

        if (m_ItemSpawnPoint != null)
            m_ItemSpawnPoint.IsFull = false;

        Destroy(gameObject);
    }
}