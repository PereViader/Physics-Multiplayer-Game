using UnityEngine;
using System.Collections;

public abstract class PlayerManager : MonoBehaviour, ISetup, IEnd
{
    [SerializeField]
    protected float respawnTime;

    public virtual void OnGameSetup()
    {
        if (PhotonNetwork.isMasterClient)
        {
            InitializePlayers();
            SpawnPlayers();
        }
    }

    public abstract void OnGameEnd();

    public virtual void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        InitializePlayer(player);
        SpawnPlayer(player);
    }

    public virtual void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        if (PhotonNetwork.isMasterClient)
        {
            GameObject playerObject = (GameObject)player.TagObject;
            PhotonNetwork.Destroy(playerObject);
        }
    }

    protected virtual void InitializePlayers()
    {
        foreach (PhotonPlayer photonPlayer in PhotonNetwork.playerList)
        {
            InitializePlayer(photonPlayer);
        }
    }

    protected abstract void InitializePlayer(PhotonPlayer player);

    protected virtual void SpawnPlayers()  // spawn dels jugadors connectats al entrar a la partida
    {
        foreach (PhotonPlayer photonPlayer in PhotonNetwork.playerList)
        {
            SpawnPlayer(photonPlayer);
        }
    }

    protected abstract void SpawnPlayer(PhotonPlayer player);

    public virtual void KillPlayer(GameObject playerObject)
    {
        int playerID = playerObject.GetComponent<PhotonRemoteOwner>().GetPlayer().ID;
        PhotonNetwork.Destroy(playerObject);
        StartCoroutine(RespawnPlayer(playerID));
    }

    protected virtual IEnumerator RespawnPlayer(int playerID)
    {
        yield return new WaitForSeconds(respawnTime);
        PhotonPlayer player = PhotonPlayer.Find(playerID);
        if (player == null)
        {
            Debug.Log("Player disconnected when trying to respawn");
        }
        else
        {
            SpawnPlayer(player);
        }
    }

    public abstract bool IsFriendly(GameObject gameObject1, GameObject gameObject2);
}
