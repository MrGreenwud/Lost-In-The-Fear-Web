using UnityEngine;
using Zenject;

public class ItemDroper : MonoBehaviour
{
    [Inject] private readonly Inventory m_Inventory;
    [Inject] private readonly InputHandler m_InputHandler;

    [SerializeField] private float m_DropDistence = 5;
    [SerializeField] private LayerMask m_GroundLayer;
    [SerializeField] private LayerMask m_SolidSurffaceLayer;

    private void Update()
    {
        if (m_InputHandler.GetDrop() == true)
            Drop();
    }

    private void Drop()
    {
        int slotIndex = m_Inventory.GetSlotSelector().SlotSelectorModel.CurrentSelectionSlotIndex;
        Slot slot = m_Inventory.GetSlots()[slotIndex];

        m_Inventory.InventoryModel.DropItem(slot, ColculateDropItemPosition());
    }

    private Vector3 ColculateDropItemPosition()
    {
        float dropDistence = m_DropDistence;

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit distenceHit, m_DropDistence, m_SolidSurffaceLayer))
            dropDistence = distenceHit.distance - 0.2f;

        Vector3 position = transform.position + transform.forward * dropDistence + Vector3.up * 3;

        if (Physics.Raycast(position, Vector3.down, out RaycastHit hit, 1000, m_GroundLayer))
            return hit.point;

        return Vector3.zero;
    }

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        float dropDistence = m_DropDistence;

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit distenceHit, m_DropDistence, m_SolidSurffaceLayer))
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, distenceHit.point);

            UnityEditor.Handles.color = Color.green;
            UnityEditor.Handles.DrawWireDisc(distenceHit.point, distenceHit.normal, 0.2f);

            dropDistence = distenceHit.distance - 0.2f;
        }

        Vector3 position = transform.position + transform.forward * dropDistence + Vector3.up * 3;

        if(Physics.Raycast(position, Vector3.down, out RaycastHit hit, 1000, m_GroundLayer))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(position, hit.point);

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(hit.point, 0.2f);
        }
    }

#endif

}
