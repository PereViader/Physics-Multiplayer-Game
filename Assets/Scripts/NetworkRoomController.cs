using UnityEngine;
using ExitGames.Client.Photon;

public class NetworkRoomController : MonoBehaviour {
    [SerializeField]
    private RoomPanelController roomPanelController;

    [SerializeField]
    private int timesToRetrySearch;

    [SerializeField]
    private float timeBetweenRetries;

    private int currentRetryCount;

    private int currentGameMode = -1;
    private bool isSearching = false;
    private bool blockFuturePlayPress = false;

    Hashtable expectedProperties;

    void Start()
    {
        expectedProperties = new Hashtable();
        PhotonNetwork.automaticallySyncScene = true;
    }

    public void joinGameOrCreate(int gameMode)
    {
        if (blockFuturePlayPress || !PhotonNetwork.connectedAndReady)
        {
            return;
        }

        if (isSearching)
            CancelInvoke();

        isSearching = true;
        roomPanelController.DisplaySearchingText();
        currentRetryCount = 0;
        currentGameMode = gameMode;
        expectedProperties.Clear();
        expectedProperties.Add(RoomProperty.Mode, gameMode);
        Debug.Log("Connected and ready" + PhotonNetwork.connectedAndReady);

        if (PhotonNetwork.inRoom)
        {
            PhotonNetwork.LeaveRoom();
            blockFuturePlayPress = true;
        } else
        {
            RetryJoinGameOrCreate();
        }
    }

    public void RetryJoinGameOrCreate()
    {
        
        if ( currentRetryCount <= timesToRetrySearch )
        {
            JoinRandomRoom();
            currentRetryCount++;
        } else
        {
            CreateRoom();
        }
    }

    void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom(expectedProperties, 0);
    }

    void OnPhotonRandomJoinFailed()
    {
        CancelInvoke();
        Invoke("RetryJoinGameOrCreate", timeBetweenRetries);
    }

    void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        string[] prop = new string[1];
        prop[0] = RoomProperty.Mode;
        roomOptions.customRoomPropertiesForLobby = prop;
        roomOptions.customRoomProperties = expectedProperties;
        roomOptions.maxPlayers = 2;
        PhotonNetwork.CreateRoom(null, roomOptions, TypedLobby.Default);
        Debug.Log("Created room");
    }

    void OnPhotonPlayerConnected()
    {
        if (PhotonNetwork.isMasterClient && PhotonNetwork.playerList.Length == 2)
        {
            Debug.Log("Load level");
            PhotonNetwork.LoadLevel(LevelProvider.GetRandomMap(currentGameMode));
        } 

    }

    void OnJoinedRoom()
    {
        roomPanelController.DisplayWaitingText();
    }

    void OnConnectedToMaster()
    {
        blockFuturePlayPress = false;
        if ( isSearching)
        {
            RetryJoinGameOrCreate();
        }
    }
}
