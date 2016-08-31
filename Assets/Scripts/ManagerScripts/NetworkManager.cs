using UnityEngine;
using System.Collections.Generic;
using ExitGames.Client.Photon;

public class NetworkManager : MonoBehaviour {

    private string gameVersion = "1";

    void Awake()
    {
        PhotonNetwork.isMessageQueueRunning = true;
    }

    void Start()
    {
        if (!PhotonNetwork.connected) // obrir la escena directament per fer proves rapidament
            Connect();
        else if ( PhotonNetwork.isMasterClient) // obrir la escena des del menu principal on ja ens haviem connectat
            InitializeGame();
    }

    void InitializeGame()
    {
        PhotonNetwork.Instantiate("GameManager", Vector3.zero, Quaternion.identity, 0);
    }

    private bool isFastDevelopmentMode = false;
    void Connect()
    {
        PhotonNetwork.player.SetCustomProperties(new Hashtable() { { "Skin", PlayerPrefs.GetString("Skin") } });
        isFastDevelopmentMode = true;
        if ( PhotonNetwork.connected )
            CreateRoom();
        else
            PhotonNetwork.ConnectUsingSettings(gameVersion);
    }
    
    void OnConnectedToMaster()
    {
        CreateRoom();
    }

    void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions() { maxPlayers = 8 };
        string[] properties = new string[1];
        properties[0] = RoomProperty.Mode;

        roomOptions.customRoomPropertiesForLobby = properties;
        Hashtable roomProperties = new Hashtable();
        roomProperties.Add(RoomProperty.Mode, GameMode.Capture);
        roomOptions.customRoomProperties = roomProperties;
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.JoinOrCreateRoom("testRoom", roomOptions, TypedLobby.Default);
    }

    void OnJoinedRoom()
    {
        if (isFastDevelopmentMode)
            InitializeGame();
    }

    void OnGUI()
    {
        GUI.Box(new Rect(0, 0, 160, 80), PhotonNetwork.connectionStateDetailed + "\n Ping: " + PhotonNetwork.GetPing()+"\n Time: "+PhotonNetwork.time+"\n Unit: "+Time.time);
    }
}
