using UnityEngine;
using System.Collections;
public class Capture_GameManager : GameManager, IScorable, IKillManager {

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

    public void Score(int team, int value)
    {
        experienceManager.AddExperienceToTeam(team, experienceManager.experienceValues.score*value);
        ((IScorable)scoreManager).Score(team, value);
        if ( scoreManager.HasGameEnded() )
        {
            int winnerTeam = scoreManager.GetWinnerTeam();
            experienceManager.AddExperienceToTeam(winnerTeam, experienceManager.experienceValues.winGame);
            OnGameEnd();
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

    public void Killed(GameObject killed, PhotonPlayer killer)
    {
        playerManager.KillPlayer(killed);
        if ( killer != null )
            experienceManager.AddExperience(killer, experienceManager.experienceValues.kill);
    }
}
