using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    PhotonPlayer lastPlayerHit;
    Capture_PlayerManager playerManager;

    [SerializeField]
    int lastHitDuration;

    WaitForSeconds removeDelay;
    PhotonRemoteOwner remoteOwner;
    void Awake()
    {
        playerManager = Component.FindObjectOfType<Capture_PlayerManager>();
        remoteOwner = GetComponent<PhotonRemoteOwner>();
        removeDelay = new WaitForSeconds(lastHitDuration);
    }


    // Matar el jugador
	void OnTriggerEnter(Collider other)
    {
        if ( other.tag == "DeadZone" && PhotonNetwork.isMasterClient)
        {
            KillPlayer();
        }
    }

    void KillPlayer()
    {
        playerManager.KillPlayer(gameObject); 
        // substituir el player manager per una porta d'entrada que cridi al playermanager la porta d'entrada també utilitzara el lastplayerhit per donar experiencia
    }


    // Establir la persona contra la que xoca per despres si mor aquesta fitxa donarli punts
    void OnCollisionEnter(Collision other)
    {
        if ( other.gameObject.tag == "Player" )
        {
            PhotonPlayer player = other.gameObject.GetComponent<PhotonRemoteOwner>().GetPlayer();
            int otherPlayerTeam = (int)player.customProperties[PlayerProperties.team];
            if ( otherPlayerTeam != (int)remoteOwner.GetPlayer().customProperties[PlayerProperties.team] )
            {
                StopAllCoroutines();
                StartCoroutine(SetAndRemovePlayer(player));
            }
        }
    }

    IEnumerator SetAndRemovePlayer(PhotonPlayer player)
    {
        lastPlayerHit = player;
        yield return removeDelay;
        lastPlayerHit = null;
    }
}
