using UnityEngine;
using System.Collections;

public class TeleportPipeEntrance : MonoBehaviour {

    [SerializeField]
    TeleportPipeExit teleportPipeExit;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            teleportPipeExit.TeleportObject(other.gameObject);
        }
    }
}
