using UnityEngine;
using System.Collections;

public class NewPlayerHealth : MonoBehaviour
{
    GameManager gameManager;

    void Awake()
    {
        gameManager = Component.FindObjectOfType<GameManager>();
    }

    // Matar el jugador
    void OnTriggerEnter(Collider other)
    {
        if (PhotonNetwork.isMasterClient && other.tag == "DeadZone")
        {
            KillPlayer();
        }
    }

    void KillPlayer()
    {
        PhotonPlayer player = GetComponent<PhotonRemoteOwner>().GetPlayer();
        gameManager.OnPlayerDeath(player,null); // TODO fer que el jugador el mati algu
    }
}
