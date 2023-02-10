using UnityEngine;
using Zenject;

public class PlayerInteractor : MonoBehaviour
{
    [Inject] private HeadRoatator m_HeadRoatator;
    [Inject] private InputHandler m_InputHandler;

    [SerializeField] private LayerMask m_InteractLayer;
    [SerializeField] private float m_Distence = 5f;

    private void Update()
    {
        if (m_InputHandler.Interact() == true)
            Interact();
    }

    private void Interact()
    {
        RaycastHit hit;
        if(Physics.Raycast(m_HeadRoatator.transform.position, m_HeadRoatator.transform.forward, out hit, m_Distence, m_InteractLayer))
        {
            if (hit.collider.TryGetComponent<Interacteble>(out Interacteble interacteble))
            {
                interacteble.Interact();

                if (ItemUser.Instance.GetSlot().SlotModel.Item == null) return;
                interacteble.Interact(ItemUser.Instance.GetSlot());
            }
        }
    }

    private void OnDrawGizmos()
    {

        if (Application.isPlaying == false) return;
        Gizmos.color = Color.green;
        Gizmos.DrawRay(new Ray(m_HeadRoatator.transform.position, m_HeadRoatator.transform.forward * m_Distence));
    }
}
