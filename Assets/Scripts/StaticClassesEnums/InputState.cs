

using UnityEngine;

public class InputState {
    public static bool isGameInput;
    public static bool isMenuInput;

    public static void ActivateMenuInput()
    {
        isMenuInput = true;
        isGameInput = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public static void ActivateGameInput()
    {
        isMenuInput = false;
        isGameInput = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
