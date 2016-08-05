using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

    [SerializeField]
    Transform objectToFollow;

    [SerializeField]
    float distanceFromObject;

    [SerializeField]
    float startingRotation;

    [SerializeField]
    float horizontalSensitivity;

    [SerializeField]
    float verticalSensitivity;

    [SerializeField]
    float lerpFactor;

    [SerializeField]
    float maximumVerticalAngle;

    [SerializeField]
    float minimumVerticalAngle;

    [SerializeField]
    float invertHorizontal;
    [SerializeField]
    float invertVertical;

    float currentHorizontalRotation;
    float currentVerticalRotation;

    bool isInputEnabled;

    void Awake()
    {
        isInputEnabled = true;
    }

    void FixedUpdate()
    {
        if (objectToFollow && isInputEnabled)
        {
            float horizontal = horizontalSensitivity * Input.GetAxis("CameraHorizontal");
            float vertical = verticalSensitivity * Input.GetAxis("CameraVertical");

            currentVerticalRotation += (invertVertical * vertical) % 360.0f;
            currentHorizontalRotation += (invertHorizontal * horizontal) % 360.0f;

            currentVerticalRotation = Mathf.Clamp(currentVerticalRotation, minimumVerticalAngle, maximumVerticalAngle);

            Vector3 desiredPositionFromObject = Quaternion.Euler(currentVerticalRotation, currentHorizontalRotation, 0f) * (Vector3.back * distanceFromObject);
            transform.position = Vector3.Lerp(transform.position, objectToFollow.position + desiredPositionFromObject, lerpFactor * Time.fixedDeltaTime);
        }
    }

    void Update()
    {
        if (objectToFollow && isInputEnabled)
            transform.LookAt(objectToFollow);
    }

    public void SetObjectToFollow(GameObject toFollow)
    {
        objectToFollow = toFollow.transform;
        currentHorizontalRotation = toFollow.transform.rotation.eulerAngles.y;
        currentVerticalRotation = startingRotation;
    }

    public void SetInput(bool state)
    {
        isInputEnabled = state;
    }
}
