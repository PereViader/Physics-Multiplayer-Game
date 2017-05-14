using UnityEngine;
using System.Collections;
using System;

public class Capture_GameManager : GameManager {

    Capture_PlayerManager playerManager;
    Capture_ScoreManager scoreManager;
    Capture_ExperienceManager experienceManager;

    void Awake()
    {
        playerManager = GetComponent<Capture_PlayerManager>();
        experienceManager = GetComponent<Capture_ExperienceManager>();
        scoreManager = GetComponent<Capture_ScoreManager>();
    }

    public void Score(int team, int value)
    {
        experienceManager.AddExperienceToTeam(team, experienceManager.experienceValues.score*value);
        scoreManager.Score(team, value);
        if (HasGameEnded())
        {
            int winnerTeam = scoreManager.GetWinnerTeam();
            experienceManager.AddExperienceToTeam(winnerTeam, experienceManager.experienceValues.winGame);
        }
        OnRoundEnd();
    }

    public override void OnPlayerDeath(PhotonPlayer deadPlayer, PhotonPlayer killer)
    {
        playerManager.OnPlayerDeath(deadPlayer);
        if (killer != null)
            experienceManager.AddExperience(killer, experienceManager.experienceValues.kill);
    }

    public override PlayerProperties.GameResult GetGameResultForPlayer(PhotonPlayer player)
    {
        PlayerProperties.GameResult gameResult = PlayerProperties.GameResult.Lose;
        int playerTeam = (int)player.customProperties[PlayerProperties.team];
        if (scoreManager.GetWinnerTeam() == playerTeam)
        {
            gameResult = PlayerProperties.GameResult.Win;
        }
        return gameResult;
    }

    public override bool HasGameEnded()
    {
        return scoreManager.HasGameEnded();
    }
}