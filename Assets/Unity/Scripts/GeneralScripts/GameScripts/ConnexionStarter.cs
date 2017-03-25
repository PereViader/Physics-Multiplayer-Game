using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class ConnexionStarter : MonoBehaviour {
    [SerializeField]
    bool isOffline;

    [SerializeField]
    GameMode gameMode;

    [SerializeField]
    bool destroyOnSetup;

    [SerializeField]
    bool displayNetworkStats;

    void Start()
    {
        if ( isOffline )
        {
            PhotonNetwork.offlineMode = isOffline;
        } else
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
    }

    void InitializePlayerForFastDevelopment()
    {
        ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
        customProperties.Add(PlayerProperties.skin, PlayerPrefs.GetString("Skin"));
        PhotonNetwork.player.SetCustomProperties(customProperties);
        PhotonNetwork.playerName = "Player " + Random.Range(0, 100);
    }

    void OnConnectedToMaster()
    {
        CreateRoom();
    }

    void CreateRoom()
    {
        /*RoomOptions roomOptions = new RoomOptions();

        roomOptions.CustomRoomPropertiesForLobby = new string[] { RoomProperty.GameMode }; // Properties visible of the room by other players
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { RoomProperty.GameMode, (int)gameMode} };

        PhotonNetwork.CreateRoom(null, roomOptions, TypedLobby.Default);*/
        RoomOptions roomOptions = GameModeFabric.ConstructRoomOptionsForGameMode(gameMode);
        TypedLobby sqlLobby = GameModeFabric.ConstructTyppedLobby();
        PhotonNetwork.CreateRoom(null, roomOptions, sqlLobby);
    }

    void OnJoinedRoom()
    {
        if (!isOffline)
        {
            PhotonNetwork.room.visible = true;
            PhotonNetwork.room.open = true;
        }
        InitializeGame();
    }

    void InitializeGame()
    {
        GetComponent<GameEventManager>().TriggerStartingGameEvents();
        if ( destroyOnSetup )
            Destroy(this);
    }

    void OnGUI()
    {
        if (displayNetworkStats)
        {
            GUILayout.Box(PhotonNetwork.connectionStateDetailed.ToString());
            GUILayout.Box("Ping: " + PhotonNetwork.GetPing());
        }
    }
}
