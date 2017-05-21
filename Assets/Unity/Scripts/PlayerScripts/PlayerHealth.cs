using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    private GameManager gameManager;
    
    private PhotonPlayer killer;

    [SerializeField]
    private float timeToDeleteKillerAfterContact;

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

    void OnCollisionEnter(Collision collision)
    {
        if ( collision.gameObject.tag.Equals("Player") )
        {
            StopAllCoroutines();
            StartCoroutine(AddAndRemovePlayerFromBeingKiller(collision.gameObject));
        }
    }

    IEnumerator AddAndRemovePlayerFromBeingKiller(GameObject killerGameObject)
    {
        this.killer = killerGameObject.GetComponent<PhotonRemoteOwner>().GetPlayer();
        yield return new WaitForSeconds(timeToDeleteKillerAfterContact);
        this.killer = null; 
    }


    void KillPlayer()
    {
        PhotonPlayer player = GetComponent<PhotonRemoteOwner>().GetPlayer();
        gameManager.OnPlayerDeath(player,killer); // TODO fer que el jugador el mati algu
    }
}
