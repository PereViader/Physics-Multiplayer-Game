using UnityEngine;
using System.Collections;

public static class InputGetter
{
    public static Vector3 getRawInputFromMasterInput()
    {
        return new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }


    public static Vector3 GetDirection()
    {
        Vector3 cameraDirection = Camera.main.transform.forward;
        cameraDirection.y = 0.0f;
        Vector3 stickDirection = getRawInputFromMasterInput();
        Quaternion shiftFromCamera = Quaternion.FromToRotation(Vector3.forward, cameraDirection);
        return shiftFromCamera * stickDirection;
    }
}
