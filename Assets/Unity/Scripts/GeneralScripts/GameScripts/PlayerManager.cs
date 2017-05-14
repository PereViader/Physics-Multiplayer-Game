using UnityEngine;
using System.Collections;
using System;

public abstract class PlayerManager : MonoBehaviour, IGame, IPlayerDeath
{
    public abstract void OnGameSetup();

    public abstract void OnGameStart();

    public abstract void OnGameEnd();

    public abstract void OnRoundSetup();

    public abstract void OnRoundStart();

    public abstract void OnRoundEnd();

    public virtual void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        //player joined the game late
        if ( PhotonNetwork.isMasterClient ) {
            InitializePlayer(player);
            SpawnPlayer(player);
        }
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
    
    public virtual void OnPlayerDeath(PhotonPlayer player)
    {
        if (player.TagObject == null)
            Debug.LogError("For some reason player " + player + " is null");
        PhotonNetwork.Destroy((GameObject)player.TagObject);
        player.TagObject = null;
    }
}
