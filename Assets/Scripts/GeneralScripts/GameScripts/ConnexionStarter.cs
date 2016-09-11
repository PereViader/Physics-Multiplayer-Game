using UnityEngine;
using System.Collections;

public class ConnexionStarter : MonoBehaviour {

    [SerializeField]
    GameMode gameMode;

    [SerializeField]
    bool destroyOnSetup;

    [SerializeField]
    bool displayNetworkStats;

    void Start()
    {

        PhotonNetwork.automaticallySyncScene = true;
        if (!PhotonNetwork.connected)
        {        // obrir la escena directament per fer proves rapidament
            InitializePlayerForFastDevelopment();
            PhotonNetwork.ConnectUsingSettings(GamePreferences.GAME_VERSION);
        }
        else if (PhotonNetwork.isMasterClient) // obrir la escena des del menu principal on ja ens haviem connectat
            InitializeGame();
    }

    void InitializePlayerForFastDevelopment()
    {
        PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { PlayerProperties.skin, PlayerPrefs.GetString("Skin") } });
    }

    void OnConnectedToMaster()
    {
        CreateRoom();
    }

    void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();

        roomOptions.CustomRoomPropertiesForLobby = new string[] { RoomProperty.GameMode }; // Properties visible of the room by other players
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { RoomProperty.GameMode, gameMode} };

        PhotonNetwork.CreateRoom(null, roomOptions, TypedLobby.Default);
    }

    void OnJoinedRoom()
    {
        InitializeGame();
    }

    void InitializeGame()
    {
        GetComponent<GameManager>().OnGameSetup();
        if ( destroyOnSetup )
            Destroy(this);
    }

    void OnGUI()
    {
        if (displayNetworkStats)
        {
            GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
            GUILayout.Label("Ping: " + PhotonNetwork.GetPing());
        }
    }
}
