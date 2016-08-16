using UnityEngine;
using System.Collections;

public class ElevatorPipeController : MonoBehaviour {

    Transform[] elevatorPath;

    [SerializeField]
    float speed;

    Vector3 exitVector;

    [SerializeField]
    float exitForce;

    // Use this for initialization
    void Awake()
    {
        Transform pathParent = transform.Find("Path");
        elevatorPath = new Transform[pathParent.childCount];
        for ( int i = 0; i < pathParent.childCount; i++)
        {
            elevatorPath[i] = pathParent.GetChild(i);
        }
        exitVector = elevatorPath[elevatorPath.Length-1].position - elevatorPath[elevatorPath.Length - 2].position;
        exitVector.Normalize();

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(MovePlayerThroughElevator(other.gameObject));
        }
    }

    IEnumerator MovePlayerThroughElevator(GameObject player)
    {
        Rigidbody playerRigidbody = player.GetComponent<Rigidbody>();
        playerRigidbody.isKinematic = true;

        foreach(Transform waypoint in elevatorPath)
        {
            while (Vector3.Distance(playerRigidbody.position, waypoint.position) > 0.1f)
            {
                playerRigidbody.position = Vector3.MoveTowards(playerRigidbody.position, waypoint.position, speed*Time.deltaTime);
                yield return null;
            }
        }
        playerRigidbody.isKinematic = false;
        playerRigidbody.AddForce(exitVector * exitForce,ForceMode.Impulse);
    }

    /*
    [SerializeField]
    float sucktionForce;

    List<Rigidbody> playersBeeingSucked;
    List<Rigidbody> playersMovingUp;

    Vector3 elevatorEntrance;

    void Awake()
    {
        playersMovingUp = new List<Rigidbody>();
    }

    
    void OnTriggerEnter(Collider other)
    {
        if ( other.tag == "Player")
        {
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            playerRb.velocity = (playerRb.velocity / 2f) + Vector3.up * sucktionForce;
        }
    }
    /*
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            playerRb.AddForce(Vector3.up * sucktionForce, ForceMode.VelocityChange);
            if ( playerRb.position.y < transform.position.y)
                playersMovingUp.Add(playerRb);
        }
    }

    void FixedUpdate()
    {
        foreach ( Rigidbody rigidbody in playersMovingUp )
        {
            rigidbody.velocity = Vector3.up * sucktionForce;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            playersMovingUp.Remove(playerRb);
        }
    }*/
}
