using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameLobyManager : MonoBehaviour {
    // UI elements
    public Text lobbyState;

    public Text masterText;

    public Text roomUsersText;

    public Transform userDisplay;

    public GameObject userTagPrefab;


    // lobby states
    public string waitingForUsersText;

    public string startingGameText;

    public string lookingForGameText;

    public string joinedLobbyText;




    // others
    public static GameMode[] desiredGameModes;


    // lobby properties
    public int playersInGame;

    public float timeToStartGame;

    private bool startingGame;


    void Awake()
    {
        PhotonNetwork.automaticallySyncScene = true;
        JoinOrCreateGame();
    }

    public void JoinOrCreateGame()
    {
        TypedLobby sqlLobby = new TypedLobby("myLobby", LobbyType.SqlLobby);

        // photon network provides sql match making. Using registers C0 .. C10 you can set your own properties
        // C0 is gamemodetype
        string sqlLobbyFilter = "";
        foreach (GameMode gameMode in desiredGameModes)
            sqlLobbyFilter += "C0 = " + (int)gameMode + ((sqlLobbyFilter != "") ? " OR " : "");
        PhotonNetwork.JoinRandomRoom(null, 0, MatchmakingMode.FillRoom, sqlLobby, sqlLobbyFilter);
    }

    // if PhotonNetwork.JoinRandomRoom fails this will be called
    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("Failed Join");
        CreateRoom(desiredGameModes[Random.Range(0, desiredGameModes.Length)]);
    }

    void CreateRoom(GameMode gameMode)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "C0", (int)gameMode } }; // C0 es el nom de la taula sql es fa servir en el matchmaking sql
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "C0" }; // this makes "C0" available in the lobby
        TypedLobby sqlLobby = new TypedLobby("myLobby", LobbyType.SqlLobby);
        Debug.Log("Try Create " + PhotonNetwork.connectedAndReady);
        PhotonNetwork.CreateRoom(null, roomOptions, sqlLobby);
    }




    void OnJoinedRoom()
    {
        lobbyState.text = joinedLobbyText;
        updatePlayerUI();
        if (PhotonNetwork.isMasterClient)
            masterText.enabled = true;
        Debug.Log("Joined");
    }

    void updatePlayerUI()
    {
        roomUsersText.text = "Players " + PhotonNetwork.playerList.Length;
        while(userDisplay.childCount > 0)
        {
            Destroy(userDisplay.GetChild(0));
        }

        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            GameObject tag = (GameObject)Instantiate(userTagPrefab,userDisplay);
            tag.GetComponent<Text>().text = player.name;
        }
    }

    void OnPhotonPlayerConnected()
    {
        updatePlayerUI();
        Debug.Log("player joined room");
        if (PhotonNetwork.playerList.Length == playersInGame)
        {
            lobbyState.text = startingGameText;
            Invoke("StartGame", timeToStartGame);
        }
    }

    void OnPhotonPlayerDisconnected()
    {
        Debug.Log("Player Disconnected");
        if ( startingGame )
        {
            startingGame = false;
            CancelInvoke();
        }
        updatePlayerUI();
    }


    void StartGame()
    {
        if(PhotonNetwork.isMasterClient)
        {
            SceneManager.LoadScene(LevelProvider.GetRandomMap((GameMode)PhotonNetwork.room.customProperties["C0"]));
        }
    }

    
}
