using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class WoodPlank : Interacteble
{
    public UnityEvent OnIteract;

    [SerializeField] private Item m_Item;

    private Rigidbody m_Rigidbody;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    public override void Interact(Slot slot)
    {
        if (p_IsInteracteble == false) return;

        if(slot.SlotModel.Item == m_Item)
        {
            p_IsInteracteble = false;
            slot.SlotModel.UseItem();
            m_Rigidbody.isKinematic = false;
            OnIteract?.Invoke();
            Destroy(gameObject);
        }
    }
}
