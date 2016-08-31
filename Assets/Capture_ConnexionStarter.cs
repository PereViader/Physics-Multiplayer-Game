using UnityEngine;
using System.Collections;

public class Capture_ConnexionStarter : MonoBehaviour {

    static string gameVersion = "2.0";

    void Start()
    {
        if (!PhotonNetwork.connected) // obrir la escena directament per fer proves rapidament
            PhotonNetwork.ConnectUsingSettings(gameVersion);
        else if (PhotonNetwork.isMasterClient) // obrir la escena des del menu principal on ja ens haviem connectat
            InitializeGame();
    }

    void OnConnectedToMaster()
    {
        CreateRoom();
    }

    void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions() { maxPlayers = 6, isOpen = true, isVisible = true };
        string[] properties = new string[] { RoomProperty.Mode };
        roomOptions.customRoomPropertiesForLobby = properties;
        ExitGames.Client.Photon.Hashtable roomProperties = new ExitGames.Client.Photon.Hashtable();
        roomProperties.Add(RoomProperty.Mode, GameMode.Capture);
        roomOptions.customRoomProperties = roomProperties;
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.CreateRoom(null, roomOptions, TypedLobby.Default);
    }

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
        GUI.Box(new Rect(0, 0, 160, 40), PhotonNetwork.connectionStateDetailed + "\n Ping: " + PhotonNetwork.GetPing());
    }
}
