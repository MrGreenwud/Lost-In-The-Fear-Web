using UnityEngine;

public class OnjectDestroyer : MonoBehaviour
{
    private bool m_IsDestroing;

    public void DestroyAction()
    {
        Destroy(gameObject);
    }
}
