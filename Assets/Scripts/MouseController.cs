using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour {



	void Awake()
    {
        SetCursorHidden(true);
    }

    public void SetCursorHidden(bool state)
    {
        if (state && !Application.isEditor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        } else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }
}
