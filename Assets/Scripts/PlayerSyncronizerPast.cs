using UnityEngine;
using System.Collections;

public class PlayerSyncronizerPast : MonoBehaviour
{

    [SerializeField]
    private float lerpFactor;

    [SerializeField]
    private float teleportThreshold;

    private Rigidbody rb;
    private Vector3 currentGoal;
    private Vector3 receivedPosition;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if ( !PhotonNetwork.isMasterClient) {
            Vector3 currentDone = Vector3.Lerp(Vector3.zero, currentGoal, lerpFactor*Time.fixedDeltaTime*(currentGoal.magnitude+1));
            currentGoal -= currentDone;
            transform.position = transform.position + currentDone;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo message)
    {
        if (stream.isWriting)
        {
            stream.SendNext(rb.position);
            stream.SendNext(rb.velocity);
        }
        else
        {
            receivedPosition = (Vector3)stream.ReceiveNext();
            currentGoal = receivedPosition - rb.position;
            if ( currentGoal.magnitude > teleportThreshold)
            {
                currentGoal = Vector3.zero;
                transform.position = receivedPosition;
            }
            rb.velocity = (Vector3)stream.ReceiveNext();
        }
    }
}