using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    CaptureGameController gameManager;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager(Clone)").GetComponent<CaptureGameController>();
    }

	void OnTriggerEnter(Collider other)
    {
        if ( other.tag == "DeadZone" && PhotonNetwork.isMasterClient)
        {
            gameManager.KillPlayer(gameObject);
        }
    } 
}
