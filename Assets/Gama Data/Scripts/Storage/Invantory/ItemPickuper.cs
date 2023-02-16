using UnityEngine;
using Zenject;

public class ItemPickuper : MonoBehaviour
{
    [Inject] private readonly InputHandler m_InputHandler;
    [Inject] private readonly Inventory m_Inventory;
    [Inject] private readonly Diary m_Diary;
    [Inject] private readonly HeadRoatator m_Head;
    [Inject] private readonly TabController m_TabController;

    [SerializeField] private float m_Distence;
    [SerializeField] private LayerMask m_ItemLayer;

    private void Update()
    {
        if (m_InputHandler.GetPickUp() == true)
            PickUp();

        if (Input.GetKeyDown(KeyCode.RightArrow))
            m_Diary.DiaryModel.ScrollForword();
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
            m_Diary.DiaryModel.ScrollBack();
    }

    private void PickUp()
    {
        RaycastHit hit;
        if (Physics.Raycast(m_Head.transform.position, m_Head.transform.forward, out hit, m_Distence, m_ItemLayer))
        {
            if (hit.collider.TryGetComponent<GameObjectItem>(out GameObjectItem item))
            {
                Debug.Log(item.name);
                m_Inventory.InventoryModel.PikUpItem(item);
            }

            if (hit.collider.TryGetComponent<GameObjectNote>(out GameObjectNote note))
            {
                Debug.Log(note.GetNote());
                m_TabController.GetReadTab().Read(note.GetNote().GetText());
                m_Diary.DiaryModel.PikUpItem(note);
                m_TabController.OpenReadTab();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (m_Head != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(m_Head.transform.position, m_Head.transform.forward * m_Distence);
        }
    }
}