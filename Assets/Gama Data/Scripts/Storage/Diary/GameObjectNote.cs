using UnityEngine;

public class GameObjectNote : MonoBehaviour
{
    [SerializeField] private Note m_Note;

    public Note GetNote() => m_Note;

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
