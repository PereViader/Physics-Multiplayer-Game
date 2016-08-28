using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    PhotonPlayer lastPlayerHit;

    [SerializeField]
    int lastHitDuration;

    WaitForSeconds removeDelay;
    PhotonPlayerOwner myself;

    void Awake()
    {
        myself = GetComponent<PhotonPlayerOwner>();
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
        PlayerManager.playerManager.KillPlayer(gameObject);
        CallKillEvents();
    }

    void CallKillEvents()
    {
        if (myself.GetOwner() == PhotonNetwork.player)
        {
            CaptureEvents.CallOnLocalPlayerKilled(gameObject);
            if (lastPlayerHit != null)
            {
                CaptureEvents.CallOnPlayerKilled(lastPlayerHit,myself.GetOwner());
            }
        }
    }


    // Establir la persona contra la que xoca per despres si mor aquesta fitxa donarli punts
    void OnCollisionEnter(Collision other)
    {
        if ( other.gameObject.tag == "Player" )
        {
            PhotonPlayerOwner owner = other.gameObject.GetComponent<PhotonPlayerOwner>();
            int otherPlayerTeam = owner.GetTeam();
            if ( otherPlayerTeam != myself.GetTeam() )
            {
                StopAllCoroutines();
                StartCoroutine(SetAndRemovePlayer(owner.GetOwner()));
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
