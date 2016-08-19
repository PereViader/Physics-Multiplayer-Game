using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    CaptureGameController gameManager;

	void OnTriggerEnter(Collider other)
    {
        if ( other.tag == "DeadZone" && PhotonNetwork.isMasterClient)
        {
            PlayerManager.playerManager.KillPlayer(gameObject);
        }
    }
}
