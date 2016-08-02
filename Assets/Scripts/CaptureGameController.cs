using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CaptureGameController : Photon.MonoBehaviour
{
    private CameraFollowSmooth cameraFollow;
    private PauseMenuManager pauseManager;

    private Transform[] capturePositions;

    private CaptureZoneController currentCapture;
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



    void Awake()
    {
        cameraFollow = GameObject.Find("Main Camera").GetComponent<CameraFollowSmooth>();
        pauseManager = GameObject.Find("Pause Menu Manager").GetComponent<PauseMenuManager>();

        team1PlayersL = new List<GameObject>();
        team2PlayersL = new List<GameObject>();

        Random.seed = ((int)System.DateTime.Now.Ticks) + ((int)Time.time);

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
    }

    void Start()
    {
        //InitializePlayer();
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
        GameObject captureZoneGameObject = (GameObject)PhotonNetwork.Instantiate("Capture Zone", capturePositions[currentCaptureIndex].position, Quaternion.identity, 0);
        currentCapture = captureZoneGameObject.GetComponent<CaptureZoneController>();
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

        if (PhotonNetwork.isMasterClient)
            InstantiateNewRandomCapture();
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
            SpawnPlayer(newPlayer);
    }

    void SpawnPlayer(PhotonPlayer player)
    {
        int team;
        int spawnIndex;
        GetValuesForNewPlayer(out team, out spawnIndex);
        Debug.Log("team: " + team);
        Transform spawn = GetSpawn(team, spawnIndex);
        GameObject playerGameObject = PhotonNetwork.Instantiate("PlayInThePastPlayer", spawn.position, spawn.rotation, 0);
        int playerViewId = playerGameObject.GetComponent<PhotonView>().viewID;
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
        properties.Add("Team", team);
        player.SetCustomProperties(properties);
        photonView.RPC("RegisterAndInitialize", PhotonTargets.AllBuffered, player.ID, playerViewId, team);
        if ( player != null)
            photonView.RPC("GiveControlToPlayer", player, playerViewId);
    }
    /*
    public void InitializePlayerObject(GameObject player, int team)
    {
        player.GetComponent<PhotonView>().RPC("SetTeam", PhotonTargets.AllBuffered, team);
    }*/

    [PunRPC]
    public void GiveControlToPlayer(int playerViewId)
    {
        GameObject player = PhotonView.Find(playerViewId).gameObject;
        player.GetComponent<PlayerControllerPast>().SetOwnPlayer(true);
        player.GetComponent<HabilityManager>().ActivateInputCaptureForHabilities();

        GameObject playerFollower = (GameObject)Instantiate((GameObject)Resources.Load("PlayerFollower"), player.transform.position, player.transform.rotation);
        playerFollower.GetComponent<PlayerFollower>().setPlayer(player);

        cameraFollow.SetFollowingObject(playerFollower);

        pauseManager.SetPlayer(player.GetComponent<PlayerControllerPast>());
        pauseManager.SetCamera(GameObject.Find("Main Camera").GetComponent<CameraFollowSmooth>());
    }

    [PunRPC]
    public void RegisterAndInitialize(int playerId, int playerViewId, int playerTeam)
    {
        GameObject player = PhotonView.Find(playerViewId).gameObject;
        player.GetComponent<PhotonPlayerOwner>().SetOwner(playerId);
        AddPlayer(player, playerTeam);
    }

    void AddPlayer(GameObject player, int playerTeam)
    {
        if (playerTeam == 1)
            team1PlayersL.Add(player);
        else
            team2PlayersL.Add(player);
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

    int GetTeam1SpawnIndex()
    {
        return Random.Range(0, team1Spawns.Length);
    }

    int GetTeam2SpawnIndex()
    {
        return Random.Range(0, team2Spawns.Length);
    }



    // ----------------------------- Others


    void OnGUI()
    {
        GUI.Box(new Rect(300, 0, 200, 40), "Team1: " + team1Points + "  || Team2: " + team2Points);
    }

    public int getTeamsInMatch()
    {
        return 2;
    }

    void OnMasterClientSwitched(PhotonPlayer newMasterClient) // si el client que fa de servidor es deconnecta tothom tanca el joc
    {
        pauseManager.OnMainMenuButtonPressed();
    }
}