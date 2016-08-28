using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerScoreManager : Photon.MonoBehaviour {
    [SerializeField]
    int scorePoints;

    [SerializeField]
    int captureHelpScore;

    [SerializeField]
    int killScore;

    [SerializeField]
    int winGame;

    [SerializeField]
    int lateJoin;

    Dictionary<PhotonPlayer, int> score;
    EndGamePanelManager endGamePanel;

    void Awake()
    {
        endGamePanel = GameObject.Find("Manager").GetComponent<EndGamePanelManager>();
        score = new Dictionary<PhotonPlayer, int>();
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            Debug.Log("dasdasd1");
            score.Add(player, 0);

        }
        CaptureEvents.OnPlayerKilled += OnPlayerKilled;
        CaptureEvents.OnGameEnded += OnMatchEnded;
        CaptureEvents.OnTeamScored += OnTeamCaptured;
    }

    void OnDestroy()
    {
        CaptureEvents.OnPlayerKilled -= OnPlayerKilled;
        CaptureEvents.OnGameEnded -= OnMatchEnded;
        CaptureEvents.OnTeamScored -= OnTeamCaptured;
        foreach (var entry in score)
        {
            Debug.Log("player: " + entry.Key.name + " score: " + entry.Value);
        }
    }

    void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        score.Remove(player);
    }

    void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        score.Add(player,lateJoin);
    }

    void OnPlayerKilled(PhotonPlayer killer, PhotonPlayer killed)
    {
        score[killer] += killScore;
    }

    

    void OnTeamCaptured(int team)
    {
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            int playerTeam = (int)player.customProperties["Team"];
            if (playerTeam == team)
            {
                score[player] += scorePoints;
            }
        }
    }

    void OnMatchEnded(int team)
    {
        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("Game end");
            foreach(PhotonPlayer player in score.Keys)
            {
                int finalScore = score[player];
                int playerTeam = (int)player.customProperties["Team"];
                if (playerTeam == team)
                {
                    finalScore += winGame;
                }
                photonView.RPC("RPC_SetScore", player, finalScore, team);
            }
        }
    }

    [PunRPC]
    void RPC_SetScore(int score, int team)
    {
        Debug.Log("rpc set score");

        endGamePanel.SetScoreAndEndGame(score,team);
    }
}
