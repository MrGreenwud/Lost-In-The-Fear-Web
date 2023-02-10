using UnityEngine;

public static class CursorState
{
    public static void Show()
    {
        Cursor.visible = true;
    }

    public static void Hide()
    {
        Cursor.visible = false;
    }

    public static void Lock()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public static void UnLock()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
