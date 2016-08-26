using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerManager : Photon.MonoBehaviour {

    public static PlayerManager playerManager;

    [SerializeField]
    int teamsInGame;

    [SerializeField]
    float respawnTime;

    List<GameObject>[] players;

    PlayerSpawnManager playerSpawnManager;

    void Awake()
    {
        if (playerManager == null)
            playerManager = this;
        else
            Debug.Log("There can only be one player manager");

        playerSpawnManager = new PlayerSpawnManager(teamsInGame);
        players = new List<GameObject>[teamsInGame];
        for (int i = 0; i < teamsInGame; i++)
            players[i] = new List<GameObject>();
    }


    // --------------------------------------------------- Initializers
    public int GetTeamForNewPlayer()
    {
        int team = -1;
        int chosenTeamMembers = -1;
        for (int currentTeam = 0; currentTeam < players.Length; currentTeam++)
        {
            if (team == -1 || players[currentTeam].Count < chosenTeamMembers)
            {
                team = currentTeam;
                chosenTeamMembers = players[currentTeam].Count;
            }
        }
        return team;
    }

    public void SendGameState(PhotonPlayer player)
    {
        foreach (PhotonPlayer other in PhotonNetwork.playerList)
        {
            if (other != player)
            {
                int playerObjectViewID = (int)other.customProperties["Object"];
                int playerTeam = (int)other.customProperties["Team"];
                photonView.RPC("RegisterPlayer", player, playerObjectViewID, playerTeam);
            }
        }
    }

    public void SpawnPlayers()  // spawn dels jugadors connectats al entrar a la partida
    {
        foreach (PhotonPlayer photonPlayer in PhotonNetwork.playerList)
        {
            SpawnPlayer(photonPlayer);
        }
    }

    // --------------------------------------------------- Player Spawning

    public void SpawnPlayer(PhotonPlayer player)
    {
        object oTeam;
        int team;
        Transform spawn;
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();

        bool teamAlreadyAssigned = player.customProperties.TryGetValue("Team", out oTeam);
        if (teamAlreadyAssigned)
            team = (int)oTeam;
        else
        {
            team = GetTeamForNewPlayer();
            properties.Add("Team", team);
        }
        spawn = playerSpawnManager.GetRandomSpawn(team);


        GameObject playerGameObject = PhotonNetwork.Instantiate("PlayerBall", spawn.position, spawn.rotation, 0);
        playerGameObject.GetComponent<HabilityManager>().AddRandomHabilities();
        int playerViewId = playerGameObject.GetComponent<PhotonView>().viewID;

        
        properties.Add("Object", playerViewId);
        player.SetCustomProperties(properties);


        photonView.RPC("RegisterPlayer", PhotonTargets.All, playerViewId, team);
        playerGameObject.GetComponent<PhotonView>().RPC("SetOwner", PhotonTargets.AllBufferedViaServer, player.ID);
    }

    // ----------------------------------------------------- Player Killers

    public void KillPlayer(GameObject playerObject)
    {
        PhotonPlayer player = playerObject.GetComponent<PhotonPlayerOwner>().GetOwner();
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
            PhotonView playerObjectView = PhotonView.Find((int)player.customProperties["Object"]);
            if (playerObjectView == null)
                Debug.LogWarning("No player object out, something went wrong!");
            else
            {
                photonView.RPC("UnregisterPlayer", PhotonTargets.All, playerObjectView.viewID, player.customProperties["Team"]);
                PhotonNetwork.Destroy(playerObjectView);
                StartCoroutine(RespawnPlayer(playerID));
            }
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




    // ---------------------------------------------------Player Tracking 
    [PunRPC]
    public void RegisterPlayer(int playerObjectViewId, int playerTeam)
    {
        PhotonView playerObjectView = PhotonView.Find(playerObjectViewId);
        if (playerObjectView == null)
            Debug.Log("playerObjectView Is null");
        else
            AddPlayer(playerObjectView.gameObject, playerTeam);
    }

    [PunRPC]
    public void UnregisterPlayer(int playerObjectViewID, int playerTeam)
    {
        PhotonView playerObjectView = PhotonView.Find(playerObjectViewID);
        if (playerObjectView == null)
            Debug.Log("playerObjectView Is null");
        else
            RemovePlayer(playerObjectView.gameObject, playerTeam);
    }

    public void AddPlayer(GameObject player, int playerTeam)
    {
        players[playerTeam].Add(player);
    }

    public void RemovePlayer(GameObject player, int playerTeam)
    {
        players[playerTeam].Remove(player);
    }

    // --------------------------------------------------- Player Getters

    public List<GameObject> GetPlayers(int team)
    {
        return players[team];
    }

    public List<GameObject> GetPlayers()
    {
        return GetOtherTeamsPlayers(-1);
    }

    public List<GameObject> GetOtherTeamsPlayers(int team)
    {
        List<GameObject> ret = new List<GameObject>();
        for(int i = 0; i<players.Length; i++)
        {
            if (team != i)
                foreach (GameObject player in players[i])
                    ret.Add(player);
        }

        return ret;
    }
}
