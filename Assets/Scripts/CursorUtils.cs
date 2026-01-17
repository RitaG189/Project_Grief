using UnityEngine;

public static class CursorUtils
{
    public static void EnableLockedCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    public static void EnableUnlockedCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public static void EnableUICursor()
    {
        Cursor.visible = true;
    }

    public static void DisableUICursor()
    {
        Cursor.visible = false;
    }
}

