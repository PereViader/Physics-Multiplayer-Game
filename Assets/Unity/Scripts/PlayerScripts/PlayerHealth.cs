using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    PhotonPlayer lastPlayerHit;
    IKillManager killManager;

    [SerializeField]
    int lastHitDuration;

    WaitForSeconds removeDelay;
    PhotonRemoteOwner remoteOwner;
    void Awake()
    {
        remoteOwner = GetComponent<PhotonRemoteOwner>();
        removeDelay = new WaitForSeconds(lastHitDuration);

        try
        {
            killManager = (IKillManager)Component.FindObjectOfType<GameManager>();
        }
        catch (System.Exception)
        {
            Debug.Log("There needs to be a GameManager that implement the IKillManager interface");
        }
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
        killManager.Killed(gameObject,lastPlayerHit); 
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
