using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CaptureGameController : Photon.MonoBehaviour
{
    public static CaptureGameController captureGameController;

    private CameraFollow cameraFollow;
    private PauseMenuManager pauseManager;

    private Transform[] capturePositions;

    private GameObject currentCapture;
    private int currentCaptureIndex = -1;

    [SerializeField]
    private float timeBetweenCaptures;

    [SerializeField]
    private Transform[] team1Spawns;

    [SerializeField]
    private Transform[] team2Spawns;

    private List<GameObject> team1PlayersL;
    private List<GameObject> team2PlayersL;

    private int team1Points;
    private int team2Points;

    Text redScore;
    Text greenScore;

    Text distanceText;
    GameObject localPlayerGameObject;
    
    void FixedUpdate()
    {

        if ( localPlayerGameObject )
            distanceText.text = Mathf.Clamp(Vector3.Distance(currentCapture.transform.position, localPlayerGameObject.transform.position)-5f,0f,1000f).ToString("0");
        else
            distanceText.gameObject.SetActive(false);
    }

    void Awake()
    {
        captureGameController = this;

        cameraFollow = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
        pauseManager = GameObject.Find("Manager").GetComponent<PauseMenuManager>();

        team1PlayersL = new List<GameObject>();
        team2PlayersL = new List<GameObject>();

        redScore = GameObject.Find("Canvas/Marcador/RedScore").GetComponent<Text>();
        greenScore = GameObject.Find("Canvas/Marcador/GreenScore").GetComponent<Text>();

        Random.seed = ((int)System.DateTime.Now.Ticks) + ((int)Time.time) + Time.frameCount;

        GameObject container = GameObject.Find("Map/CapturePositions");
        capturePositions = new Transform[container.transform.childCount];
        for (int i = 0; i < container.transform.childCount; i++)
        {
            capturePositions[i] = container.transform.GetChild(i);
        }

        container = GameObject.Find("Map/SpawnPositions/Team 1");
        team1Spawns = new Transform[container.transform.childCount];
        for (int i = 0; i < container.transform.childCount; i++)
        {
            team1Spawns[i] = container.transform.GetChild(i);
        }

        container = GameObject.Find("Map/SpawnPositions/Team 2");
        team2Spawns = new Transform[container.transform.childCount];
        for (int i = 0; i < container.transform.childCount; i++)
        {
            team2Spawns[i] = container.transform.GetChild(i);
        }

        distanceText = GameObject.Find("Canvas").transform.Find("DistanceText").GetComponent<Text>();
    }

    void Start()
    {
        if (PhotonNetwork.isMasterClient)
        {
            SpawnPlayers();
            InstantiateNewRandomCapture();
        }
    }
    
//----------------------------------------------- Capture Zones
    void InstantiateNewRandomCapture()
    {
        if (currentCaptureIndex != -1)
        {
            PhotonNetwork.Destroy(currentCapture.gameObject);
        }

        currentCaptureIndex = getDiferentRandomCapturePosition();
        currentCapture = (GameObject)PhotonNetwork.Instantiate("Capture Zone", capturePositions[currentCaptureIndex].position, Quaternion.identity, 0);
        currentCapture.GetComponent<AreaController>().SetGameManager(this);
    }

    int getDiferentRandomCapturePosition()
    {
        int capturePosition = (int)Random.Range(0f, capturePositions.Length);
        while (capturePosition == currentCaptureIndex)
            capturePosition = (int)Random.Range(0f, capturePositions.Length);
        return capturePosition;
    }


//-------------------------------------------------   Points
    [PunRPC]
    public void TeamScored(int team)
    {
        if (team == 1) 
            team1Points++;
        else
            team2Points++;

        UpdateScoreUI();

        if (PhotonNetwork.isMasterClient)
            InstantiateNewRandomCapture();
    }

    void UpdateScoreUI()
    {
        redScore.text = team1Points.ToString();
        greenScore.text = team2Points.ToString();
    }

//------------------------------------------------- Player Spawning
    void SpawnPlayers()  // spawn dels jugadors connectats al entrar a la partida
    {
        foreach (PhotonPlayer photonPlayer in PhotonNetwork.playerList)
        {
            SpawnPlayer(photonPlayer);
        }
    }

    void OnPhotonPlayerConnected(PhotonPlayer newPlayer) // spawn d'un jugador nou que acaba de conectar
    {
        if (PhotonNetwork.isMasterClient)
        {
            SpawnPlayer(newPlayer);
            SendGameState(newPlayer);
        }
    }

    void SendGameState(PhotonPlayer player)
    {
        foreach ( PhotonPlayer other in PhotonNetwork.playerList)
        {
            if ( other != player )
            {
                int playerObjectViewID = (int)other.customProperties["Object"];
                int playerTeam = (int)other.customProperties["Team"];
                photonView.RPC("RegisterPlayer", player, playerObjectViewID, playerTeam);
            }
        }
    }

    void SpawnPlayer(PhotonPlayer player)
    {
        int team;
        int spawnIndex;
        GetValuesForNewPlayer(out team, out spawnIndex);
        Transform spawn = GetSpawn(team, spawnIndex);
        GameObject playerGameObject = PhotonNetwork.Instantiate("PlayInThePastPlayer", spawn.position, spawn.rotation, 0);
        int playerViewId = playerGameObject.GetComponent<PhotonView>().viewID;
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
        properties.Add("Team", team);
        properties.Add("Object", playerViewId);
        player.SetCustomProperties(properties);
        playerGameObject.GetComponent<PhotonView>().RPC("SetOwner", PhotonTargets.AllBuffered, player.ID);
        photonView.RPC("RegisterPlayer", PhotonTargets.All, playerViewId, team);
        photonView.RPC("GiveControlToPlayer", player, playerViewId);
    }

    [PunRPC]
    public void GiveControlToPlayer(int playerViewId)
    {
        distanceText.gameObject.SetActive(true);
        GameObject player = PhotonView.Find(playerViewId).gameObject;
        localPlayerGameObject = player;
        player.GetComponent<PlayerControllerPast>().SetOwnPlayer(true);
        player.GetComponent<HabilityManager>().ActivateInputCaptureForHabilities();

        GameObject playerFollower = (GameObject)Instantiate((GameObject)Resources.Load("PlayerFollower"), player.transform.position, player.transform.rotation);
        playerFollower.GetComponent<PlayerFollower>().setPlayer(player);

        cameraFollow.SetObjectToFollow(playerFollower);

        pauseManager.SetPlayer(player.GetComponent<PlayerControllerPast>());
        pauseManager.SetCamera(GameObject.Find("Main Camera").GetComponent<CameraFollow>());
    }

    [PunRPC]
    void RegisterPlayer(int playerObjectViewId, int playerTeam)
    {
        PhotonView playerObjectView = PhotonView.Find(playerObjectViewId);
        if (playerObjectView == null)
            Debug.Log("playerObjectView Is null");
        else
            AddPlayer(playerObjectView.gameObject, playerTeam);
    }

    [PunRPC]
    void UnregisterPlayer(int playerObjectViewID, int playerTeam)
    {
        PhotonView playerObjectView = PhotonView.Find(playerObjectViewID);
        if (playerObjectView == null)
            Debug.Log("playerObjectView Is null");
        else
            RemovePlayer(playerObjectView.gameObject, playerTeam);
    }

    void AddPlayer(GameObject player, int playerTeam)
    {
        if (playerTeam == 1)
            team1PlayersL.Add(player);
        else
            team2PlayersL.Add(player);
    }

    void RemovePlayer(GameObject player, int playerTeam)
    {
        if (playerTeam == 1)
            team1PlayersL.Remove(player);
        else
            team2PlayersL.Remove(player);
    }

    public List<GameObject> GetPlayers(int team)
    {
        switch (team)
        {
            case 1:
                return team1PlayersL;
            case 2:
                return team2PlayersL;
            default:
                return null;
        }
    }

    public List<GameObject> GetOtherTeamsPlayers(int team)
    {
        switch (team)
        {
            case 1:
                return team2PlayersL;
            case 2:
                return team1PlayersL;
            default:
                return null;
        }
    }

    Transform GetSpawn(int team, int spawnIndex)
    {
        Transform transform;
        if (team == 1)
            transform = team1Spawns[spawnIndex];
        else
            transform = team2Spawns[spawnIndex];
        return transform;
    }

    public void GetValuesForNewPlayer(out int team, out int spawnIndex)
    {
        if (team1PlayersL.Count > team2PlayersL.Count)
            team = 2;
        else if (team1PlayersL.Count < team2PlayersL.Count)
            team = 1;
        else // team1Players == team2Playersw
            team = Random.Range(1, 3);

        if (team == 1)
            spawnIndex = GetTeam1SpawnIndex();
        else
            spawnIndex = GetTeam2SpawnIndex();
    }

    public int GetSpawnIndex(int team)
    {
        int spawnIndex;
        if (team == 1)
            spawnIndex = GetTeam1SpawnIndex();
        else
            spawnIndex = GetTeam2SpawnIndex();
        return spawnIndex;
    }

    int GetTeam1SpawnIndex()
    {
        return Random.Range(0, team1Spawns.Length);
    }

    int GetTeam2SpawnIndex()
    {
        return Random.Range(0, team2Spawns.Length);
    }


    // -------------------------------- Player Killing and respawning

    public void KillPlayer(GameObject playerObject)
    {
        PhotonPlayer player = playerObject.GetComponent<PhotonPlayerOwner>().GetOwner();
        photonView.RPC("NotifyPlayerKilledAndQueueRespawn", PhotonTargets.MasterClient, player.ID);
    }

    void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        int playerTeam = (int)otherPlayer.customProperties["Team"];
        PhotonView playerObjectView = PhotonView.Find((int)otherPlayer.customProperties["Object"]);
        photonView.RPC("UnregisterPlayer", PhotonTargets.All, playerObjectView.viewID, playerTeam);
        PhotonNetwork.Destroy(playerObjectView);
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
                photonView.RPC("UnregisterPlayer", PhotonTargets.All,playerObjectView.viewID,player.customProperties["Team"]);
                PhotonNetwork.Destroy(playerObjectView);
                StartCoroutine(WaitAndRespawnPlayer(playerID));
            }
        }
    }

    IEnumerator WaitAndRespawnPlayer(int playerID)
    {
        yield return new WaitForSeconds(3f);
        PhotonPlayer player = PhotonPlayer.Find(playerID);
        if (player == null)
            Debug.Log("Player Disconnected");
        else {
            RespawnPlayer(player);
        }
    }

    void RespawnPlayer(PhotonPlayer player)
    {
        if ( player == null )
        {
            Debug.Log("Player disconnected when trying to respawn");
        } else
        {
            int team = (int)player.customProperties["Team"];
            int spawnIndex = GetSpawnIndex(team);
            Transform spawn = GetSpawn(team, spawnIndex);
            GameObject playerGameObject = PhotonNetwork.Instantiate("PlayInThePastPlayer", spawn.position, spawn.rotation, 0);
            int playerViewId = playerGameObject.GetComponent<PhotonView>().viewID;
            ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
            properties.Add("Object", playerViewId);
            player.SetCustomProperties(properties);
            playerGameObject.GetComponent<PhotonView>().RPC("SetOwner", PhotonTargets.AllBuffered, player.ID);
            photonView.RPC("RegisterPlayer", PhotonTargets.All, playerViewId, team);
            photonView.RPC("GiveControlToPlayer", player, playerViewId);
        }
    }



    // ----------------------------- Others

    public int getTeamsInMatch()
    {
        return 2;
    }

    void OnMasterClientSwitched(PhotonPlayer newMasterClient) // si el client que fa de servidor es deconnecta tothom tanca el joc
    {
        pauseManager.OnMainMenuButtonPressed();
    }
}