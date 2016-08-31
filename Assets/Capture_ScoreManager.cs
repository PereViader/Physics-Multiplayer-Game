using UnityEngine;
using System.Collections;
using System;

public class Capture_ScoreManager : Photon.MonoBehaviour, IScorable {

    [SerializeField]
    int teamsInGame;

    [SerializeField]
    int endGameScore;

    int[] score;

    CaptureUI_ScoreManager scoreManagerUI;
    bool hasGameEnded;
    int winnerTeam;

    void Awake()
    {
        winnerTeam = -1;
        score = new int[teamsInGame];
        scoreManagerUI = Component.FindObjectOfType<CaptureUI_ScoreManager>();
    }

    //---------------------------------------

    public void Score(int team, int value = 1)
    {
        if ( !hasGameEnded )
            photonView.RPC("RPC_Score", PhotonTargets.All, team, value);
    }

    [PunRPC]
    void RPC_Score(int team, int value)
    {
        score[team] += value;
        scoreManagerUI.SetScore(team, score[team]);
        if (score[team] >= endGameScore)
            SetEndGame(team);
    }
    //---------------------------------------

    public void SetScore(int team, int score)
    {
        if (!hasGameEnded)
            photonView.RPC("RPC_SetScore", PhotonTargets.All, team, score);
    }

    [PunRPC]
    void RPC_SetScore(int team, int score)
    {
        this.score[team] = score;
        scoreManagerUI.SetScore(team, score);
        if ( score >= endGameScore)
            SetEndGame(team);
    }

    //---------------------------------------

    public void SetScore(int[] score)
    {
        if (!hasGameEnded)
            photonView.RPC("RPC_SetScore", PhotonTargets.All, score);
    }

    [PunRPC]
    void RPC_SetScore(int[] score)
    {
        score.CopyTo(this.score, 0);
        scoreManagerUI.SetScore(score);
        for (int i = 0; i < teamsInGame; i++)
        {
            if (score[i] >= endGameScore)
            {
                SetEndGame(i);
                break;
            }
        }
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
