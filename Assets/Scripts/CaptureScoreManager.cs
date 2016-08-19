using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CaptureScoreManager : Photon.MonoBehaviour {

    public static CaptureScoreManager captureScoreManager;

    Text[] scoreUI;
    int[] score;

    CaptureZoneManager captureZoneManger;

    void Awake()
    {
        if (captureScoreManager == null)
            captureScoreManager = this;
        else
            Debug.Log("There can only be one capture score manager");

        scoreUI = new Text[2];
        score = new int[2];
        scoreUI[0] = GameObject.Find("Canvas/Marcador/RedScore").GetComponent<Text>();
        scoreUI[1] = GameObject.Find("Canvas/Marcador/GreenScore").GetComponent<Text>();

        captureZoneManger = GetComponent<CaptureZoneManager>();
        CaptureEvents.OnTeamScored += ScorePoint;
    }

    void OnDestroy()
    {
        CaptureEvents.OnTeamScored -= ScorePoint;
    }

    

    public void ScorePoint(int team)
    {
        photonView.RPC("RPC_ScorePoint", PhotonTargets.All, team);
        captureZoneManger.InstantiateNewRandomCapture();
    }

    [PunRPC]
    void RPC_ScorePoint(int team)
    {
        score[team] += 1;
        scoreUI[team].text = score[team].ToString();
    }


    public void SendGameState(PhotonPlayer player)
    {
        photonView.RPC("RPC_SetScore", player, score);
    }

    [PunRPC]
    void RPC_SetScore(int[] score)
    {
        this.score = score;
        for ( int i = 0; i < score.Length; i++)
        {
            scoreUI[i].text = score[i].ToString();
        }
    }
}
