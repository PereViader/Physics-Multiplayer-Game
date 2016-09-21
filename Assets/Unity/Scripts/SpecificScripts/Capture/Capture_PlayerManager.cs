using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Capture_PlayerManager : PlayerManager {
    [SerializeField]
    int teamsInGame;

    Capture_ScoreManager scoreManager;
    Capture_SpawnManager spawnManager;
    CaptureUI_CameraManager cameraManager;

    void Awake()
    {
        scoreManager = GetComponent<Capture_ScoreManager>();
        spawnManager = GetComponent<Capture_SpawnManager>();
        cameraManager = Component.FindObjectOfType<CaptureUI_CameraManager>();
    }

    public override void OnGameEnd()
    {
        if ( PhotonNetwork.isMasterClient)
        {
            int winnerTeam = scoreManager.GetWinnerTeam();
            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
                int playerTeam = (int)player.customProperties[PlayerProperties.team];
                PlayerProperties.GameResult result;
                if (playerTeam == winnerTeam)
                {
                    result = PlayerProperties.GameResult.Win;
                }
                else
                {
                    result = PlayerProperties.GameResult.Lose;
                }
                Debug.Log("Player " + player.ID + " " + result);
                player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { PlayerProperties.gameResult, result } });
            }
        }
    }

    protected override void InitializePlayer(PhotonPlayer player)
    {
        int playerTeam = GetTeamForNewPlayer();
        player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { PlayerProperties.team, playerTeam } });
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
        int playerTeam = (int)player.customProperties[PlayerProperties.team];
        Transform spawn = spawnManager.GetRandomSpawn(playerTeam);
        GameObject playerObject = PhotonNetwork.InstantiateSceneObject("GameMode/Capture/Player", spawn.position, spawn.rotation, 0, new object[] { player.ID });
        cameraManager.SetPlayer(playerObject);
    }

    public override bool IsFriendly(GameObject gameObject1, GameObject gameObject2)
    {
        int player1Team = (int)gameObject1.GetComponent<PhotonRemoteOwner>().GetPlayer().customProperties[PlayerProperties.team];
        int player2Team = (int)gameObject2.GetComponent<PhotonRemoteOwner>().GetPlayer().customProperties[PlayerProperties.team];
        return player1Team == player2Team;
    }

    // --------------------------------------------------- Player Getters
    /*
    public static List<GameObject> GetPlayers(int team)
    {
        List<GameObject> players = new List<GameObject>();
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
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
            if (player.TagObject != null)
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
    }*/
}
