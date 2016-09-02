using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Capture_PlayerManager : MonoBehaviour {

    [SerializeField]
    int teamsInGame;

    [SerializeField]
    float respawnTime;

    Capture_ScoreManager scoreManager;
    Capture_SpawnManager spawnManager;
    CaptureUI_CameraManager cameraManager;

    void Awake()
    {
        scoreManager = GetComponent<Capture_ScoreManager>();
        spawnManager = GetComponent<Capture_SpawnManager>();
        cameraManager = Component.FindObjectOfType<CaptureUI_CameraManager>();
    }

	public void OnGameModeSetup()
    {
        if ( PhotonNetwork.isMasterClient)
        {
            InitializePlayers();
            SpawnPlayers();
        }
    }

    public void OnGameModeEnded()
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

    public void PlayerConnected(PhotonPlayer player)
    {

        InitializePlayer(player);
        SpawnPlayer(player);
    }

    void Update()
    {
        if ( Input.GetKeyDown(KeyCode.Space))
        {
            GetPlayersTeams();
        }
    }

    // -------------------------------- Player Initializers

    void InitializePlayers()
    {
        foreach (PhotonPlayer photonPlayer in PhotonNetwork.playerList)
        {
            InitializePlayer(photonPlayer);
        }
    }

    void InitializePlayer(PhotonPlayer player)
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

    public void SpawnPlayers()  // spawn dels jugadors connectats al entrar a la partida
    {
        foreach (PhotonPlayer photonPlayer in PhotonNetwork.playerList)
        {
            SpawnPlayer(photonPlayer);
        }
    }

    public void SpawnPlayer(PhotonPlayer player)
    {
        int playerTeam = (int)player.customProperties[PlayerProperties.team];
        Transform spawn = spawnManager.GetRandomSpawn(playerTeam);
        GameObject playerObject = PhotonNetwork.InstantiateSceneObject("GameMode/Capture/Player", spawn.position, spawn.rotation, 0, new object[] { player.ID });
        cameraManager.SetPlayer(playerObject);
    }

    public void PlayerDisconnected(PhotonPlayer player)
    {
        Debug.Log("PlayerDisconnected");
        Debug.Log("PlayerList "+PhotonNetwork.playerList.Length);

        if (PhotonNetwork.isMasterClient)
        {
            GameObject playerObject = (GameObject)player.TagObject;
            PhotonNetwork.Destroy(playerObject);
        }
    }

    public void KillPlayer(GameObject playerObject)
    {

        int playerID = playerObject.GetComponent<PhotonRemoteOwner>().GetPlayer().ID;
        PhotonNetwork.Destroy(playerObject);
        StartCoroutine(RespawnPlayer(playerID));
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
    }
}
