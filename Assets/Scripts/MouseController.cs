using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour {

    [SerializeField]
    bool isActive;

	void Awake()
    {
        SetCursorHidden(true);
    }

    public void SetCursorHidden(bool state)
    {
        if (isActive)
            if ( state && !Application.isEditor)
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
