using UnityEngine;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

public class NetworkRoomController : MonoBehaviour {
    [SerializeField]
    private RoomPanelController roomPanelController;

    [SerializeField]
    private int timesToRetrySearch;

    [SerializeField]
    private float timeBetweenRetries;

    private int currentRetryCount;

    private bool isSearching = false;
    private bool blockFuturePlayPress = false;

    Hashtable expectedProperties;

    void Start()
    {
        expectedProperties = new Hashtable();
        PhotonNetwork.automaticallySyncScene = true;
    }

    public void joinGameOrCreate(GameMode gameMode)
    {
        if (blockFuturePlayPress || !PhotonNetwork.connectedAndReady)
        {
            return;
        }

        if (isSearching)
            CancelInvoke();

        isSearching = true;
        roomPanelController.DisplaySearchingText();

        expectedProperties.Clear();
        expectedProperties.Add(RoomProperty.GameMode, gameMode);


        currentRetryCount = 0;

        if (PhotonNetwork.inRoom)
        {
            PhotonNetwork.LeaveRoom();
        } else
        {
            RetryJoinGameOrCreate();
        }
        blockFuturePlayPress = true;
    }


    public void RetryJoinGameOrCreate()
    {
        
        if ( currentRetryCount <= timesToRetrySearch )
        {
            PhotonNetwork.JoinRandomRoom(expectedProperties, 0);
            currentRetryCount++;
        } else
        {
            CreateRoom();
        }
    }

    void OnPhotonRandomJoinFailed()
    {
        blockFuturePlayPress = false;
        CancelInvoke();
        Invoke("RetryJoinGameOrCreate", timeBetweenRetries);
    }

    void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();

        roomOptions.CustomRoomPropertiesForLobby = new string[] { RoomProperty.GameMode }; // Properties visible of the room from the lobby used for matchmaking
        roomOptions.CustomRoomProperties = expectedProperties; // Properties of the room 

        PhotonNetwork.CreateRoom(null, roomOptions, TypedLobby.Default);
    }

    void OnPhotonPlayerConnected()
    {
        if (PhotonNetwork.isMasterClient && PhotonNetwork.playerList.Length == 2)
        {
            SceneManager.LoadScene(LevelProvider.GetRandomMap((GameMode)PhotonNetwork.room.customProperties[RoomProperty.GameMode]));
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

    void OnGUI()
    {
    }
}
