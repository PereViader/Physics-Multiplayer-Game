﻿using UnityEngine;
using System.Collections;

public static class GameModeFabric {
    public static int GetPlayersForGameMode(GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameMode.Capture:
                return 2;
            case GameMode.KingOfTheHill:
                return 2;
            default:
                return 0;
        }
    }

    public static RoomOptions ConstructRoomOptionsForGameMode(GameMode gameMode)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)GetPlayersForGameMode(gameMode);
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { RoomProperties.GameMode, (int)gameMode } }; // C0 es el nom de la taula sql es fa servir en el matchmaking sql
        roomOptions.CustomRoomPropertiesForLobby = new string[] { RoomProperties.GameMode }; // this makes RoomProperty.GameMode available in the lobby
        return roomOptions;
    }

    public static TypedLobby ConstructTypedLobby()
    {
        return new TypedLobby("gameLobby", LobbyType.SqlLobby);
    }
}
