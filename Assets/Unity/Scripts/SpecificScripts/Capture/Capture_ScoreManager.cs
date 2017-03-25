using UnityEngine;
using System.Collections;
using System;

public class Capture_ScoreManager : Photon.MonoBehaviour, IGame {

    [SerializeField]
    int teamsInGame;

    [SerializeField]
    int endGameScore;

    int[] score;

    bool hasGameEnded;
    int winnerTeam;

    void Awake()
    {
        winnerTeam = -1;
        score = new int[teamsInGame];
    }

    public void OnGameSetup()
    {
        if(PhotonNetwork.isMasterClient)
        {
            InitializeGameScore();
        }
    }

    public void InitializeGameScore()
    {
        for (int team = 0; team < teamsInGame; team++)
        {
            ExitGames.Client.Photon.Hashtable customProperties = PhotonNetwork.room.customProperties;
            customProperties[RoomProperties.Score+team] = new int[teamsInGame];
            PhotonNetwork.room.SetCustomProperties(customProperties);
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

    public void Score(int team, int value = 1)
    {
        int currentScore = (int)PhotonNetwork.room.customProperties[RoomProperties.Score + team];
        PhotonNetwork.room.customProperties[RoomProperties.Score + team] = currentScore + value;
        if (score[team] >= endGameScore)
            SetEndGame(team);
    }

    void SetEndGame(int winner)
    {
        hasGameEnded = true;
        winnerTeam = winner;
    }

    public bool HasGameEnded()
    {
        return hasGameEnded;
    }
 
    public int GetWinnerTeam()
    {
        return winnerTeam;
    }


}
