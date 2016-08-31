using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CaptureGameController : Photon.MonoBehaviour
{
    public static CaptureGameController captureGameController;

    PlayerManager playerManager;
    CaptureZoneManager captureZoneManager;
    CaptureScoreManager captureScoreManager;

    void Awake()
    {
        CaptureEvents.OnGameEnded += OnGameEnded;
        if (captureGameController == null)
            captureGameController = this;
        else
            Debug.Log("There can't be more than one capture game controller");

        Random.seed = ((int)System.DateTime.Now.Ticks) + ((int)Time.time) + Time.frameCount;

        playerManager = GetComponent<PlayerManager>();
        captureZoneManager = GetComponent<CaptureZoneManager>();
        captureScoreManager = GetComponent<CaptureScoreManager>();
    }

    void Start()
    {
        if (PhotonNetwork.isMasterClient)
        {
            playerManager.InitializePlayers();
            playerManager.SpawnPlayers();
            captureZoneManager.InstantiateNewRandomCapture();
        }
    }    

    void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        int playerTeam = (int)otherPlayer.customProperties["Team"];
        PhotonView playerObjectView = PhotonView.Find((int)otherPlayer.customProperties["Object"]);
        photonView.RPC("UnregisterPlayer", PhotonTargets.All, playerObjectView.viewID, playerTeam);
        PhotonNetwork.Destroy(playerObjectView);
    }

    void OnPhotonPlayerConnected(PhotonPlayer newPlayer) // spawn d'un jugador nou que acaba de conectar
    {
        if (PhotonNetwork.isMasterClient)
        {
            captureScoreManager.SendGameState(newPlayer);
            playerManager.InitializePlayer(newPlayer);
            playerManager.SpawnPlayer(newPlayer);
        }
    }

    void OnGameEnded(int winnerTeam)
    {
        playerManager.SetPlayersGameResult(winnerTeam);
        PhotonNetwork.LoadLevel("EndGameScene");
    }

    void OnMasterClientSwitched(PhotonPlayer newMasterClient) // si el client que fa de servidor es deconnecta tothom tanca el joc
    {
        PauseMenuManager.pauseMenuManager.OnMainMenuButtonPressed();
    }
}