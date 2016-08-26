using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    PhotonPlayer lastPlayerHit;

    [SerializeField]
    int lastHitDuration;

    WaitForSeconds removeDelay;

    void Awake()
    {
        removeDelay = new WaitForSeconds(lastHitDuration);
    }

	void OnTriggerEnter(Collider other)
    {
        if ( other.tag == "DeadZone" && PhotonNetwork.isMasterClient)
        {
            KillPlayer();
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if ( other.gameObject.tag == "Player" )
        {
            PhotonPlayerOwner owner = other.gameObject.GetComponent<PhotonPlayerOwner>();
            int team = owner.GetTeam();
            if ( team != (int)PhotonNetwork.player.customProperties["Team"])
            {
                StopCoroutine("SetAndRemovePlayer");
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

    void KillPlayer()
    {
        PlayerManager.playerManager.KillPlayer(gameObject);
        CallKillEvents();
    }

    void CallKillEvents()
    {
        if (GetComponent<PhotonPlayerOwner>().GetOwner() == PhotonNetwork.player)
        {
            CaptureEvents.CallOnLocalPlayerKilled(gameObject);
            if ( lastPlayerHit != null )
            {
                CaptureEvents.CallOnPlayerKilled(lastPlayerHit);
            }
        }
    }
}
