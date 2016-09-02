using UnityEngine;
using System.Collections;

public class Capture_ConnexionStarter : MonoBehaviour {
    void Start()
    {

        PhotonNetwork.automaticallySyncScene = true;
        if (!PhotonNetwork.connected) {        // obrir la escena directament per fer proves rapidament
            InitializePlayerForFastDevelopment();
            PhotonNetwork.ConnectUsingSettings(GamePreferences.GAME_VERSION);
        }
        else if (PhotonNetwork.isMasterClient) // obrir la escena des del menu principal on ja ens haviem connectat
            InitializeGame();
    }

    void InitializePlayerForFastDevelopment()
    {
        PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "Skin", PlayerPrefs.GetString("Skin") } });
    }

    void OnConnectedToMaster()
    {
        CreateRoom();
    }

    void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();

        roomOptions.customRoomPropertiesForLobby = new string[] { RoomProperty.GameMode }; // Properties visible of the room by other players
        roomOptions.customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { RoomProperty.GameMode, GameMode.Capture } };

        PhotonNetwork.CreateRoom("asd", roomOptions, TypedLobby.Default);
    }
    /*
    void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        string[] prop = new string[] { RoomProperty.Mode };
        roomOptions.customRoomPropertiesForLobby = prop;
        roomOptions.customRoomProperties = expectedProperties;
        roomOptions.maxPlayers = 2;
        PhotonNetwork.CreateRoom(null, roomOptions, TypedLobby.Default);
    }*/

    void OnJoinedRoom()
    {
        InitializeGame();
    }

    void InitializeGame()
    {
        GetComponent<Capture_GameManager>().OnGameModeSetup();
        //Destroy(this);
    }

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        GUILayout.Label("Ping: " + PhotonNetwork.GetPing());
    }
}
