using UnityEngine;
using System.Collections;

public class PlayerSyncronizer : MonoBehaviour {

    [SerializeField]
    private float pingFactor = 1.5f;
    [SerializeField]
    private float correctiveValue;
    [SerializeField]
    private float correctiveReductor;
    [SerializeField]
    private float teleportThresHold;

    //--------------- Variables
    private Vector3 realOldPosition;
    private Vector3 realOldVelocity;
    private int realOldMovementId;
    private double realOldTime;
    private float elapsedTime;
    private Vector3 estimatedCurrentPosition;
    private Vector3 estimatedCurrentVelocity;
    private Vector3 correctiveDirection;
    

    private long messageNum = long.MinValue;
    private long mostRecentMessageNum = long.MinValue;
    private long receivedMessageNum;

    private Rigidbody rb;
    private PlayerController playerController;
    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        estimatedCurrentPosition = transform.position;
        estimatedCurrentVelocity = Vector3.zero;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if ( !PhotonNetwork.isMasterClient)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + correctiveDirection, correctiveValue);
            correctiveDirection = correctiveDirection * correctiveReductor;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo message)
    {
        if (stream.isWriting)
        {
            stream.SendNext(messageNum++);
            stream.SendNext(playerController.getCurrentMovementId());
            stream.SendNext(transform.position);
            stream.SendNext(rb.velocity);
        }
        else {
            if (!Input.GetKey(KeyCode.JoystickButton1))
            {
                receivedMessageNum = (long)stream.ReceiveNext();
                if (receivedMessageNum < mostRecentMessageNum)
                    return;
                realOldMovementId = (int)stream.ReceiveNext();
                realOldPosition = (Vector3)stream.ReceiveNext();
                realOldVelocity = (Vector3)stream.ReceiveNext();
                elapsedTime = (PhotonNetwork.GetPing() / 2000f) * pingFactor;
                estimatedCurrentPosition = realOldPosition + realOldVelocity * elapsedTime;
                estimatedCurrentVelocity = realOldVelocity * (1 - elapsedTime * rb.drag);

                if (playerController.getCurrentMovementId() == realOldMovementId )
                {
                    correctiveDirection = estimatedCurrentPosition - rb.position;

                    if ( correctiveDirection.magnitude > teleportThresHold)
                    {
                        UpdatePosition();
                        correctiveDirection = Vector3.zero;
                    }
                }
            }
        }
    }

    void UpdatePosition()
    {
        Debug.Log("Teleport");
        rb.position = estimatedCurrentPosition;
        rb.velocity = estimatedCurrentVelocity;
    }
}
