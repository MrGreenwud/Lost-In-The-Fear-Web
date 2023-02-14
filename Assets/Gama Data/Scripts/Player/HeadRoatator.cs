using UnityEngine;
using Zenject;

public class HeadRoatator : MonoBehaviour
{
    [Inject] private PlayerController m_PlayerController;

    [SerializeField] private float m_Sensetivity = 10f;

    [Space(10)]

    [SerializeField] private float m_MinClamp = -90f;
    [SerializeField] private float m_MaxClamp = 90f;

    private float m_RotationX;
    private float m_OriginRotationX;

    private bool m_IsLocked;

    private void Awake()
    {
        CursorState.Hide();
        CursorState.Lock();

        m_Sensetivity = Settings.s_Sensetivity;
    }

    private void OnEnable()
    {
        m_RotationX = 0;
    }

    private void Update()
    {
        Rotate();
    }

    public void Lock()
    {
        m_IsLocked = true;
        m_OriginRotationX = m_RotationX;
    }

    public void UnLock()
    {
        m_IsLocked = false;
        m_RotationX = m_OriginRotationX;
    }

    private void Rotate()
    {
        if (m_IsLocked == true) return;

        float x = m_PlayerController.InputHandler.GetHeadRotatinAxis().x * m_Sensetivity * Time.deltaTime;
        float y = m_PlayerController.InputHandler.GetHeadRotatinAxis().y * m_Sensetivity * Time.deltaTime;

        m_RotationX -= y;
        m_RotationX = Mathf.Clamp(m_RotationX, m_MinClamp, m_MaxClamp);

        transform.localRotation = Quaternion.Euler(m_RotationX, 0, 0);
        m_PlayerController.transform.Rotate(Vector3.up * x);
    }
}
