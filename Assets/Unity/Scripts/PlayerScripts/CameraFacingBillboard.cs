using UnityEngine;

public class CameraFacingBillboard : MonoBehaviour
{
    Camera m_Camera;

    Vector3 parentOffset;

    void Awake()
    {
        parentOffset = transform.localPosition;
        m_Camera = Camera.main;
    }

    void Update()
    {
        transform.position = transform.parent.position + parentOffset;
        transform.LookAt(
            transform.position + m_Camera.transform.forward,
            m_Camera.transform.up
            );
    }
}