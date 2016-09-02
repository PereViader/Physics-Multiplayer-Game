using UnityEngine;
using System.Collections;

public class Capture_GameManager : MonoBehaviour, IScorable {

    Capture_PlayerManager playerManager;
    Capture_ScoreManager scoreManager;
    Capture_ExperienceManager experienceManager;
    Capture_AreaManager areaManager;

    void Awake()
    {
        Random.seed = ((int)System.DateTime.Now.Ticks) + ((int)Time.time) + Time.frameCount;
        playerManager = GetComponent<Capture_PlayerManager>();
        experienceManager = GetComponent<Capture_ExperienceManager>();
        scoreManager = GetComponent<Capture_ScoreManager>();
        areaManager = GetComponent<Capture_AreaManager>();
    }

    public void OnGameModeSetup()
    {
        experienceManager.OnGameModeSetup();
        playerManager.OnGameModeSetup();
        areaManager.OnGameModeSetup();
    }

    public void OnGameModeEnded()
    {
        experienceManager.OnGameModeEnded();
        playerManager.OnGameModeEnded();

        if ( PhotonNetwork.isMasterClient )
        {
            PhotonNetwork.LoadLevel("EndGameScene");
        }
    }

    public void Score(int team, int value)
    {
        experienceManager.AddExperienceToTeam(team, experienceManager.experienceValues.score*value);
        ((IScorable)scoreManager).Score(team, value);
        if ( scoreManager.HasGameEnded() )
        {
            int winnerTeam = scoreManager.GetWinnerTeam();
            experienceManager.AddExperienceToTeam(winnerTeam, experienceManager.experienceValues.winGame);
            OnGameModeEnded();
        } else 
            areaManager.InstantiateNewRandomCapture();
    }

    public void SetScore(int team, int value)
    {
        ((IScorable)scoreManager).SetScore(team, value);
    }

    public void SetScore(int[] score)
    {
        ((IScorable)scoreManager).SetScore(score);
    }

    void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        playerManager.PlayerConnected(newPlayer);
        scoreManager.PlayerConnected(newPlayer);
    }

    void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        experienceManager.PlayerDisconnected(player);
        playerManager.PlayerDisconnected(player);
    }
}
