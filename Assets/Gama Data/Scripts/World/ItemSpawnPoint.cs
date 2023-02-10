using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ItemSpawnPoint : MonoBehaviour
{
    public bool IsFull;

    public void SetIsTrigger()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }
}
