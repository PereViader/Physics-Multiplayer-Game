using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(PhotonView))]
public class KingOfTheHill_GameManager : GameManager {

    [SerializeField]
    private int scoreToWin;

    [SerializeField]
    private float endGameDelay;

    private KingOfTheHill_PlayerManager playerManager;
    private KingOfTheHill_ExperienceManager experienceManager;

    void Awake()
    {
        playerManager = GetComponent<KingOfTheHill_PlayerManager>();
        experienceManager = GetComponent<KingOfTheHill_ExperienceManager>();
        InputState.ActivateGameInput();
    }

    public override void OnPlayerDeath(PhotonPlayer deadPlayer, PhotonPlayer killer)
    {
        if ( killer != null )
            experienceManager.AddExperience(killer, experienceManager.eliminate);

        playerManager.OnPlayerDeath(deadPlayer);
        PhotonPlayer winner;
        if (CheckEndOfRound(out winner))
        {
            if ( winner != null )
            {
                IncreasePlayerScore(winner);
                experienceManager.AddExperience(winner, experienceManager.winRound);
                if (HasGameEnded())
                    experienceManager.AddExperience(winner, experienceManager.winGame);
            }
            OnRoundEnd();
        }
    }

    public override PlayerProperties.GameResult GetGameResultForPlayer(PhotonPlayer player)
    {
        PlayerProperties.GameResult gameResult = PlayerProperties.GameResult.Lose;
        if ((int)PhotonNetwork.player.customProperties[PlayerProperties.score] == scoreToWin)
        {
            gameResult = PlayerProperties.GameResult.Win;
        }
        return gameResult;
    }

    private void IncreasePlayerScore(PhotonPlayer player)
    {
        ExitGames.Client.Photon.Hashtable customProperties = player.customProperties;
        customProperties[PlayerProperties.score] = 1 + (int)customProperties[PlayerProperties.score];
        player.SetCustomProperties(customProperties);
    }

    private bool CheckEndOfRound(out PhotonPlayer roundWinner)
    {
        int playersAlive = 0;
        roundWinner = null;
        foreach( PhotonPlayer player in PhotonNetwork.playerList)
        {
            if (player.TagObject != null)
            {
                playersAlive++;
                if (playersAlive <= 1)
                {
                    roundWinner = player;
                } else
                {
                    roundWinner = null;
                    break;
                }
            }
        }
        
        return playersAlive <= 1;
    }

    public override bool HasGameEnded()
    {
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            if ((int)player.customProperties[PlayerProperties.score] == scoreToWin)
            {
                return true;
            }
        }
        return false;
    }
}