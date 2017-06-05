using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class GameLobbyManager : MonoBehaviour {
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

    // Lobby GameMode
    public static GameMode[] desiredGameModes;
    public static GameMode chosenGameMode;


    // lobby properties
    public float timeToStartGame;

    private bool startingGame;

    void Awake()
    {
        PhotonNetwork.automaticallySyncScene = true;
        JoinOrCreateGame();
    }

    void JoinOrCreateGame()
    {
        if ( desiredGameModes != null )
        {
            TypedLobby sqlLobby = GameModeFabric.ConstructTypedLobby();
            string[] sqlLobbyGameModeOptions = desiredGameModes.Select(x => RoomProperties.GameMode + "=" + (int)x).ToArray();
            string sqlLobbyFilter = string.Join(" OR ", sqlLobbyGameModeOptions);
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
        CreateRoom(desiredGameModes[Random.Range(0, desiredGameModes.Length)]);
    }

    void CreateRoom(GameMode gameMode)
    {
        RoomOptions roomOptions = GameModeFabric.ConstructRoomOptionsForGameMode(gameMode);
        TypedLobby sqlLobby = GameModeFabric.ConstructTypedLobby();
        PhotonNetwork.CreateRoom(null, roomOptions, sqlLobby);
    }

    // called after random join or create room
    void OnJoinedRoom()
    {
        PhotonNetwork.room.open = true;
        PhotonNetwork.room.visible = true;
        if (PhotonNetwork.playerList.Length == GameModeFabric.GetPlayersForGameMode((GameMode)PhotonNetwork.room.customProperties[RoomProperties.GameMode]))
            lobbyState.text = startingGameText;
        else
            lobbyState.text = waitingForUsersText;
        updatePlayerUI();
        if (PhotonNetwork.isMasterClient)
            masterText.enabled = true;
    }

    void updatePlayerUI()
    {
        roomUsersText.text = "Players " + PhotonNetwork.playerList.Length + " / "+ GameModeFabric.GetPlayersForGameMode((GameMode)PhotonNetwork.room.customProperties[RoomProperties.GameMode]);
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
        if (PhotonNetwork.playerList.Length == GameModeFabric.GetPlayersForGameMode((GameMode)PhotonNetwork.room.customProperties[RoomProperties.GameMode]))
        {
            CancelInvoke();
            lobbyState.text = startingGameText;
            startingGame = true;
            Invoke("StartGame", timeToStartGame);
        }
    }

    void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
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
            SceneManager.LoadScene(LevelProvider.GetRandomMap((GameMode)PhotonNetwork.room.customProperties[RoomProperties.GameMode]));
        }
    }

    public void ExitGame()
    {
        if (PhotonNetwork.connectedAndReady)
            PhotonNetwork.LeaveRoom();
        else
            LoadMainMenu();
    }

    public void OnLeftRoom()
    {
        LoadMainMenu();
    }

    void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
