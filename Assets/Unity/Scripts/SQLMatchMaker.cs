using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SQLMatchMaker : MonoBehaviour {

    GameMode chosenGameModeForRoomCreation;

    void Awake()
    {
        PhotonNetwork.automaticallySyncScene = true;
    }

    public void JoinOrCreateGame(GameMode[] gameModes)
    {
        chosenGameModeForRoomCreation = gameModes[Random.Range(0, gameModes.Length)];
        TypedLobby sqlLobby = new TypedLobby("myLobby", LobbyType.SqlLobby);    // same as above
        string sqlLobbyFilter = "";
        foreach(GameMode gameMode in gameModes)
            sqlLobbyFilter += "C0 = "+ (int)gameMode + ((sqlLobbyFilter != "") ? " OR " : "");
        PhotonNetwork.JoinRandomRoom(null, 0, MatchmakingMode.FillRoom , sqlLobby, sqlLobbyFilter);
    }

    void OnJoinedRoom()
    {
        Debug.Log("Joined");
    }

    void OnPhotonPlayerConnected()
    {
        Debug.Log("player joined room");
        if (PhotonNetwork.isMasterClient && PhotonNetwork.playerList.Length == 2)
        {
            SceneManager.LoadScene(LevelProvider.GetRandomMap(chosenGameModeForRoomCreation));
        }
    }

    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("Failed Join");
        CreateRoom(chosenGameModeForRoomCreation);
    }

    void CreateRoom(GameMode gameMode)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "C0", (int)gameMode } }; // C0 es el nom de la taula sql es fa servir en el matchmaking sql
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "C0" }; // this makes "C0" available in the lobby
        TypedLobby sqlLobby = new TypedLobby("myLobby", LobbyType.SqlLobby);
        Debug.Log("Try Create "+ PhotonNetwork.connectedAndReady);
        PhotonNetwork.CreateRoom(null, roomOptions, sqlLobby);
    }

}
