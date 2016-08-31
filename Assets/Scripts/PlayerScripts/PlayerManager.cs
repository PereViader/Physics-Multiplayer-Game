using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerManager : Photon.MonoBehaviour {

    public static PlayerManager playerManager;

    [SerializeField]
    int teamsInGame;

    [SerializeField]
    float respawnTime;

    PlayerSpawnManager playerSpawnManager;

    void Awake()
    {
        if (playerManager == null)
            playerManager = this;
        else
            Debug.Log("There can only be one player manager");

        playerSpawnManager = new PlayerSpawnManager(teamsInGame);
    }

    public void SetPlayersGameResult(int winnerTeam)
    {
        foreach(PhotonPlayer player in PhotonNetwork.playerList)
        {
            int playerTeam = (int)player.customProperties["Team"];
            int result;
            if ( playerTeam == winnerTeam )
            {
                result = 1;
            } else
            {
                result = 0;
            }
            player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { PlayerProperties.gameResult, result } });
        }
    }

    // --------------------------------------------------- Initializers
    public int GetTeamForNewPlayer()
    {
        int[] teamPlayers = GetPlayersTeams();
        int minTeam = 0;
        for (int i = 0; i < teamsInGame; i++)
        {
            if (teamPlayers[i] < teamPlayers[minTeam]) { minTeam = i; }
        }
        return minTeam;
    }


    int[] GetPlayersTeams()
    {
        int[] teamPlayers = new int[teamsInGame];
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            if (player.customProperties[PlayerProperties.team] != null)
            {
                int playerTeam = (int)player.customProperties[PlayerProperties.team];
                teamPlayers[playerTeam] += 1;
            }
        }
        return teamPlayers;
    }

    public void SpawnPlayers()  // spawn dels jugadors connectats al entrar a la partida
    {
        foreach (PhotonPlayer photonPlayer in PhotonNetwork.playerList)
        {
            SpawnPlayer(photonPlayer);
        }
    }

    // -------------------------------------------------- Player Initializing

    public void InitializePlayers()
    {
        foreach (PhotonPlayer photonPlayer in PhotonNetwork.playerList)
        {
            InitializePlayer(photonPlayer);
        }
    }

    public void InitializePlayer(PhotonPlayer player)
    {
        int playerTeam = GetTeamForNewPlayer();
        Debug.Log("Chosen team " + playerTeam);
        player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { PlayerProperties.team, playerTeam } });
    }


    // --------------------------------------------------- Player Spawning

    public void SpawnPlayer(PhotonPlayer player)
    {
        int playerTeam = (int)player.customProperties["Team"];
        Transform spawn = playerSpawnManager.GetRandomSpawn(playerTeam);
        PhotonNetwork.InstantiateSceneObject("PlayerBall", spawn.position, spawn.rotation, 0, new object[] { player.ID });
    }

    // ----------------------------------------------------- Player Killers

    public void KillPlayer(GameObject playerObject)
    {
        PhotonPlayer player = playerObject.GetComponent<Capture_Initializer>().GetPlayer();
        photonView.RPC("NotifyPlayerKilledAndQueueRespawn", PhotonTargets.MasterClient, player.ID);
    }

    [PunRPC]
    void NotifyPlayerKilledAndQueueRespawn(int playerID)
    {
        PhotonPlayer player = PhotonPlayer.Find(playerID);
        if (player == null)
            Debug.Log("Player Disconnected");
        else
        {
            GameObject playerObject = (GameObject)player.TagObject;
            PhotonNetwork.Destroy(playerObject);
            StartCoroutine(RespawnPlayer(playerID));
        }
    }

    IEnumerator RespawnPlayer(int playerID)
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

    // --------------------------------------------------- Player Getters
    
    public static List<GameObject> GetPlayers(int team)
    {
        List<GameObject> players = new List<GameObject>();
        foreach ( PhotonPlayer player in PhotonNetwork.playerList)
        {
            int playerTeam = (int)player.customProperties["Team"];
            if (playerTeam == team && player.TagObject != null)
                players.Add((GameObject)player.TagObject);
        }
        return players;
    }

    public static List<GameObject> GetPlayers()
    {
        List<GameObject> players = new List<GameObject>();
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            if ( player.TagObject != null)
                players.Add((GameObject)player.TagObject);
        }
        return players;
    }

    public static List<GameObject> GetOtherTeamsPlayers(int team)
    {
        List<GameObject> players = new List<GameObject>();
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            int playerTeam = (int)player.customProperties["Team"];
            if (playerTeam != team && player.TagObject != null)
                players.Add((GameObject)player.TagObject);
        }
        return players;
    }
}
