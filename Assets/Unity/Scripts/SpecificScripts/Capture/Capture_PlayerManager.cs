using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Capture_PlayerManager : NewPlayerManager, IPlayerDeath {
    [SerializeField]
    private int teamsInGame;

    [SerializeField]
    private float playerRespawnDelay;

    SpawnProvider spawnProvider;

    void Awake()
    {
        spawnProvider = GetComponent<SpawnProvider>();
    }

    public override void OnGameSetup() {
        if (PhotonNetwork.isMasterClient)
                InitializePlayers();
    }

    public override void OnGameStart() {
        if (PhotonNetwork.isMasterClient)
                SpawnPlayers();
    }

    public override void OnRoundSetup() {}

    public override void OnRoundStart() {}

    public override void OnRoundEnd() {}

    public override void OnGameEnd() {}

    protected override void InitializePlayer(PhotonPlayer player)
    {
        int playerTeam = GetTeamForNewPlayer();
        player.customProperties[PlayerProperties.team] = playerTeam;
        player.SetCustomProperties(player.customProperties);
    }

    // ----------------------------------- Team Getters
    int GetTeamForNewPlayer()
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

    // ------------------------------------------------ Player Spawners

    protected override void SpawnPlayer(PhotonPlayer player)
    {
        Transform spawn = spawnProvider.GetFreeSpawn();
        PhotonNetwork.InstantiateSceneObject("GameMode/Player", spawn.position, spawn.rotation, 0, new object[] { player.ID });
    }

    public bool IsFriendly(GameObject gameObject1, GameObject gameObject2)
    {
        int player1Team = (int)gameObject1.GetComponent<PhotonRemoteOwner>().GetPlayer().customProperties[PlayerProperties.team];
        int player2Team = (int)gameObject2.GetComponent<PhotonRemoteOwner>().GetPlayer().customProperties[PlayerProperties.team];
        return player1Team == player2Team;
    }

    public override void OnPlayerDeath(PhotonPlayer player)
    {
        base.OnPlayerDeath(player);
        StartCoroutine(RespawnPlayer(player));
    }

    IEnumerator RespawnPlayer(PhotonPlayer player)
    {
        yield return new WaitForSeconds(playerRespawnDelay);
        if ( !player.isInactive )
        {
            SpawnPlayer(player);
        }
    }
}
