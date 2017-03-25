using UnityEngine;
using System.Collections;
using System;

public class Capture_ScoreManager : Photon.MonoBehaviour, IGame {

    [SerializeField]
    int teamsInGame;

    [SerializeField]
    int endGameScore;

    public void OnGameSetup()
    {
        if(PhotonNetwork.isMasterClient)
        {
            InitializeGameScore();
        }
    }

    public void OnGameStart()
    {
    }

    public void OnRoundSetup()
    {
    }

    public void OnRoundStart()
    {
    }

    public void OnRoundEnd()
    {
    }

    public void OnGameEnd()
    {
    }

    public void InitializeGameScore()
    {
        ExitGames.Client.Photon.Hashtable customProperties = PhotonNetwork.room.customProperties;
        for (int team = 0; team < teamsInGame; team++)
        {
            customProperties[RoomProperties.Score + team] = 0;
        }
        PhotonNetwork.room.SetCustomProperties(customProperties);
    }

    public void Score(int team, int value = 1)
    {
        int currentScore = (int)PhotonNetwork.room.customProperties[RoomProperties.Score + team];
        PhotonNetwork.room.customProperties[RoomProperties.Score + team] = currentScore + value;
        PhotonNetwork.room.SetCustomProperties(PhotonNetwork.room.customProperties);
    }

    public bool HasGameEnded()
    {
        return GetWinnerTeam() >= 0;
    }
 
    public int GetWinnerTeam()
    {
        for (int team = 0; team < this.teamsInGame; team++)
        {
            int teamScore = (int)PhotonNetwork.room.customProperties[RoomProperties.Score + team];
            if (teamScore >= this.endGameScore)
                return team;
        }
        return -1;
    }
}
