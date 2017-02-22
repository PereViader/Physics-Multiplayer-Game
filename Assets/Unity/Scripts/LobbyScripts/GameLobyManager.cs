using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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



    // others
    public static GameMode[] desiredGameModes;


    // lobby properties
    public static int playersInGame;

    public float timeToStartGame;

    private bool startingGame;


    void Awake()
    {
        PhotonNetwork.automaticallySyncScene = true;
        JoinOrCreateGame();
    }

    public void JoinOrCreateGame()
    {
        if ( desiredGameModes != null )
        {
            TypedLobby sqlLobby = new TypedLobby("myLobby", LobbyType.SqlLobby);

            // photon network provides sql match making. Using registers C0 .. C10 you can set your own properties
            // C0 is gamemodetype
            string sqlLobbyFilter = "";
            foreach (GameMode gameMode in desiredGameModes)
                sqlLobbyFilter += ((sqlLobbyFilter != "") ? " OR " : "")+"C0 = " + (int)gameMode;
            Debug.Log("Trying to join random room");
            Debug.Log(sqlLobbyFilter);
            PhotonNetwork.JoinRandomRoom(null, 0, MatchmakingMode.FillRoom, sqlLobby, sqlLobbyFilter);
        } else
        {
            Debug.Log("No desired modes specified");
            SceneManager.LoadScene("MainMenu");
        }
    }

    void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        masterText.enabled = PhotonNetwork.isMasterClient;
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
        PhotonNetwork.CreateRoom(null, roomOptions, sqlLobby);
    }

    // called after random join or create room
    void OnJoinedRoom()
    {
        if (PhotonNetwork.playerList.Length == playersInGame)
            lobbyState.text = startingGameText;
        else
            lobbyState.text = waitingForUsersText;
        updatePlayerUI();
        if (PhotonNetwork.isMasterClient)
            masterText.enabled = true;
        Debug.Log("Joined");
    }

    void updatePlayerUI()
    {
        roomUsersText.text = "Players " + PhotonNetwork.playerList.Length + " / "+playersInGame;
        List<Transform> tags = new List<Transform>();
        for (int child = 0; child < userDisplay.childCount; child++)
        {
            tags.Add(userDisplay.GetChild(child));
        }

        tags.ForEach(x => Destroy(x.gameObject));

        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            GameObject tag = (GameObject)Instantiate(userTagPrefab,userDisplay);
            tag.GetComponent<Text>().text = player.name;
        }
    }

    void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        updatePlayerUI();
        Debug.Log("player joined room");
        if (PhotonNetwork.playerList.Length == playersInGame)
        {
            CancelInvoke();
            lobbyState.text = startingGameText;
            startingGame = true;
            Invoke("StartGame", timeToStartGame);
        }
    }

    void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        Debug.Log("Player Disconnected");
        if ( startingGame )
        {
            startingGame = false;
            CancelInvoke();
        }
        lobbyState.text = waitingForUsersText;
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
