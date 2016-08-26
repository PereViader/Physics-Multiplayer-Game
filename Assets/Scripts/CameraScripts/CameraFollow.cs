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

    [SerializeField]
    Vector3 lookAtOffset;

    [SerializeField]
    bool useLinearCamera;

    float currentHorizontalRotation;
    float currentVerticalRotation;

    bool isInputEnabled;

    Vector3 lastPosition;
    bool hasLastPositionBeenSet;

    void Awake()
    {
        hasLastPositionBeenSet = false;
        isInputEnabled = true;
    }

    void FixedUpdate()
    {
        if (objectToFollow || hasLastPositionBeenSet )
        {
            Vector3 position = objectToFollow != null ? objectToFollow.position : lastPosition;

            float horizontal = 0f;
            float vertical = 0f;
            if ( isInputEnabled && !PauseMenuManager.isPausePanelActive)
            {
                horizontal = horizontalSensitivity * Input.GetAxis("CameraHorizontal");
                vertical = verticalSensitivity * Input.GetAxis("CameraVertical");
            }

            currentVerticalRotation += (invertVertical * vertical) % 360.0f;
            currentVerticalRotation = Mathf.Clamp(currentVerticalRotation, minimumVerticalAngle, maximumVerticalAngle);

            currentHorizontalRotation += (invertHorizontal * horizontal) % 360.0f;

            Vector3 desiredPositionFromObject = Quaternion.Euler(currentVerticalRotation, currentHorizontalRotation, 0f) * (Vector3.back * distanceFromObject);
            if ( useLinearCamera )
            {
                transform.position = Vector3.Lerp(transform.position, position + desiredPositionFromObject, lerpFactor * Time.fixedDeltaTime);
            } else
            {
                Vector3 currentPositionFromObject = transform.position - position;
                transform.position = Vector3.Slerp(currentPositionFromObject, desiredPositionFromObject, lerpFactor * Time.fixedDeltaTime) + position;
            }
            lastPosition = position;
        }
    }

    void Update()
    {
        if (objectToFollow || hasLastPositionBeenSet)
            transform.LookAt(lastPosition + lookAtOffset);
    }

    public void SetObjectToFollow(GameObject toFollow)
    {
        objectToFollow = toFollow.transform;
        currentHorizontalRotation = toFollow.transform.rotation.eulerAngles.y;
        currentVerticalRotation = startingRotation;
        lastPosition = toFollow.transform.position;
        hasLastPositionBeenSet = true;
    }

    public void SetInput(bool state)
    {
        isInputEnabled = state;
    }
}
