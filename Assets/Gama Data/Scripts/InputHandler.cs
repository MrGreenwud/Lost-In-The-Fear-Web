using System;
using UnityEngine;
using NaughtyAttributes;
using UnityEditor;

public class InputHandler : MonoBehaviour
{
    public enum DeviceInput { Desktop, Mobile }
    [SerializeField] private DeviceInput Device;

    #region Desktop

    [Header("Inputs")]

    [Space(5)]
    [Header("Move")]

    [ShowIf("Device", DeviceInput.Desktop)]
    [SerializeField] private string m_MoveHorizontalAxis = "Horizontal";

    [ShowIf("Device", DeviceInput.Desktop)]
    [SerializeField] private string m_MoveVerticalAxis = "Vertical";

    [Space(10)]

    [ShowIf("Device", DeviceInput.Desktop)]
    [SerializeField] private KeyCode m_Run = KeyCode.LeftShift;

    [ShowIf("Device", DeviceInput.Desktop)]
    [SerializeField] private KeyCode m_Crouch = KeyCode.LeftControl;

    [ShowIf("Device", DeviceInput.Desktop)]
    [SerializeField] private KeyCode m_Jump = KeyCode.Space;

    [Space(10)]
    [Header("Head Roattion")]

    [ShowIf("Device", DeviceInput.Desktop)]
    [SerializeField] private string m_LoockXAxis = "Mouse X";

    [ShowIf("Device", DeviceInput.Desktop)]
    [SerializeField] private string m_LoockYAxis = "Mouse Y";

    [Space(10)]
    [Header("Tab")]

    [ShowIf("Device", DeviceInput.Desktop)]
    [SerializeField] private KeyCode m_NoteList = KeyCode.L;

    [Space(10)]
    [Header("Interact with Envirement")]

    [ShowIf("Device", DeviceInput.Desktop)]
    [SerializeField] private KeyCode m_PickUp = KeyCode.E;

    [ShowIf("Device", DeviceInput.Desktop)]
    [SerializeField] private KeyCode m_Drop = KeyCode.G;

    [ShowIf("Device", DeviceInput.Desktop)]
    [SerializeField] private KeyCode m_Interact = KeyCode.E;

    [ShowIf("Device", DeviceInput.Desktop)]
    [SerializeField] private KeyCode m_Use = KeyCode.Mouse0;

    [ShowIf("Device", DeviceInput.Desktop)]
    [SerializeField] private KeyCode m_SelectSlot1 = KeyCode.Alpha1;

    [ShowIf("Device", DeviceInput.Desktop)]
    [SerializeField] private KeyCode m_SelectSlot2 = KeyCode.Alpha2;

    [ShowIf("Device", DeviceInput.Desktop)]
    [SerializeField] private KeyCode m_SelectSlot3 = KeyCode.Alpha3;

    [ShowIf("Device", DeviceInput.Desktop)]
    [SerializeField] private KeyCode m_SelectSlot4 = KeyCode.Alpha4;

    [ShowIf("Device", DeviceInput.Desktop)]
    [SerializeField] private KeyCode m_SelectSlot5 = KeyCode.Alpha5;

    #endregion

    public DeviceInput GetDevice() => Device;

    public Vector2 GetMoveAxis()
    {
        float horizontal = 0;
        float vertical = 0;

        if (Device == DeviceInput.Desktop)
        {
            horizontal = Input.GetAxis(m_MoveHorizontalAxis);
            vertical = Input.GetAxis(m_MoveVerticalAxis);
        }

        return new Vector2(horizontal, vertical);
    }

    public Vector2 GetHeadRotatinAxis()
    {
        float loockX = 0;
        float loockY = 0;

        if(Device == DeviceInput.Desktop)
        {
            loockX = Input.GetAxis(m_LoockXAxis);
            loockY = Input.GetAxis(m_LoockYAxis);
        }

        return new Vector2(loockX, loockY);
    }

    public bool GetRun()
    {
        return Input.GetKey(m_Run);
    }

    public bool GetCrouch()
    {
        return Input.GetKey(m_Crouch);
    }

    public bool GetJump()
    {
        return Input.GetKeyDown(m_Jump);
    }

    public bool GetNoteList()
    {
        return Input.GetKeyDown(m_NoteList);
    }

    public bool GetPickUp()
    {
        return Input.GetKeyDown(m_PickUp);
    }

    public bool GetDrop()
    {
        return Input.GetKeyDown(m_Drop);
    }

    public bool Interact()
    {
        return Input.GetKeyDown(m_Interact);
    }

    public bool Use()
    {
        return Input.GetKeyDown(m_Use);
    }

    public bool ScrollUp()
    {
        return Input.mouseScrollDelta.y > 0;
    }

    public bool ScrollDown()
    {
        return Input.mouseScrollDelta.y < 0;
    }

    public int SelectSlot(int selectonSlot)
    {
        if (Input.GetKeyDown(m_SelectSlot1))
            selectonSlot = 0;
        else if (Input.GetKeyDown(m_SelectSlot2))
            selectonSlot = 1;
        else if (Input.GetKeyDown(m_SelectSlot3))
            selectonSlot = 2;
        else if (Input.GetKeyDown(m_SelectSlot4))
            selectonSlot = 3;
        else if (Input.GetKeyDown(m_SelectSlot5))
            selectonSlot = 4;

        return selectonSlot;
    }
}
