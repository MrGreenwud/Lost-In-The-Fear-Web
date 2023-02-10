using UnityEngine;
using NaughtyAttributes;

public class Door : Interacteble
{
    public enum Side
    {
        Forword,
        Back
    }

    [SerializeField] private GameObject m_SideBase;

    private GameObject m_Forword;
    private GameObject m_Back;

    [Space(10)]

    [SerializeField] private DoorSide m_ForwordSide;
    [SerializeField] private DoorSide m_BackSide;

    [Space(10)]

    [SerializeField] private float m_OpenAngle = 90;
    [SerializeField] private float m_CloseAngle = 0;

    [Space(5)]

    [SerializeField] private float m_RotatinSpeed = 10f;

    [Space(5)]

    [SerializeField] private bool m_IsLocked = false;

    [ShowIf("m_IsLocked")]
    [SerializeField] private int m_KeyID;

    private float m_OpenAngleWithCurrentSide;

    private bool m_IsOpen;
    private Side m_CurrentPlayerPosition;

    [Button("Create Sides")]
    private void CreateSides()
    {
        if(m_SideBase == null)
        {
            Debug.LogError("SideBase prefab is null!");
            return;
        }

        if(m_Forword != null)
            Debug.LogWarning("Ceating new Forword Side!");

        if(m_Back != null)
            Debug.LogWarning("Ceating new Back Side!");

        m_Forword = CreatSide("Forword Side", transform.position + transform.forward);
        m_Back = CreatSide("Back Side", transform.position - transform.forward);

        m_ForwordSide = m_Forword.GetComponent<DoorSide>();
        m_BackSide = m_Back.GetComponent<DoorSide>();
    }

    private void OnEnable()
    {
        m_ForwordSide.OnEnter += EnterForword;
        m_BackSide.OnEnter += EnterBack;
    }

    private void OnDisable()
    {
        m_ForwordSide.OnEnter -= EnterForword;
        m_BackSide.OnEnter -= EnterBack;
    }

    private void Update()
    {
        if (m_IsOpen == true)
            Open();
        else
            Close();
    }

    public override void Interact()
    {
        if (m_IsLocked == true) return;

        ColculateOpenAngle();

        m_IsOpen = !m_IsOpen;

        base.Interact();
    }

    public override void Interact(Slot slot)
    {
        if (m_IsLocked == false) return;

        if(slot.SlotModel.Item.GetID() == m_KeyID)
        {
            UnLock();
            base.Interact(slot);
        }
    }

    private void EnterForword()
    {
        m_CurrentPlayerPosition = Side.Forword;
    }

    private void EnterBack()
    {
        m_CurrentPlayerPosition = Side.Back;
    }

    private GameObject CreatSide(string name, Vector3 position)
    {
        GameObject newSide;
        newSide = Instantiate(m_SideBase, position, Quaternion.identity);
        newSide.transform.parent = transform.parent;
        newSide.name = name;

        return newSide;
    }

    private void Open()
    {
        Rotate(m_OpenAngleWithCurrentSide);
    }

    private void Close()
    {
        Rotate(m_CloseAngle);
    }

    public void OpenAction()
    {
        ColculateOpenAngle();
        m_IsOpen = true;
    }

    public void CloseAction()
    {
        m_IsOpen = false;
    }

    public void Lock()
    {
        m_IsLocked = true;
    }

    public void UnLock()
    {
        m_IsLocked = false;
    }

    private void ColculateOpenAngle()
    {
        float angle = 0;

        if (m_CurrentPlayerPosition == Side.Forword)
            angle = m_OpenAngle;
        else
            angle = -m_OpenAngle;

        m_OpenAngleWithCurrentSide = transform.rotation.y + angle;
    }

    private void Rotate(float angleY)
    {
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.localRotation.x, angleY, transform.localRotation.z), Time.deltaTime * m_RotatinSpeed);
    }
    
}
