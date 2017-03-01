using UnityEngine;
using System.Collections;

public class NewPlayerHealth : MonoBehaviour
{
    IPlayerDeath killManager;

    void Awake()
    {
        killManager = (IPlayerDeath)Component.FindObjectOfType<NewPlayerManager>();
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
        killManager.OnPlayerDeath(player);
    }
}
