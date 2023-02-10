using UnityEngine;

public class HeapDirt : Interacteble
{
    [SerializeField] private Item m_Item;

    public override void Interact(Slot slot)
    {
        if(slot.SlotModel.Item == m_Item)
        {
            if (p_IteractCount < 2)
                transform.localScale = transform.localScale / 1.5f;
            else
                Destroy(gameObject);

            slot.SlotModel.UseItem();
            base.Interact(slot);
        }
    }

    public override void Interact() { }
}
