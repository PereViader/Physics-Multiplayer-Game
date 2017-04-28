using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(PhotonView))]
public class KingOfTheHill_GameManager : GameManager {

    [SerializeField]
    private int scoreToWin;

    [SerializeField]
    private int roundWinExperience;

    [SerializeField]
    private int gameWinExperience;

    [SerializeField]
    private float endGameDelay;

    [SerializeField]
    private Transform deadZone;

    private KingOfTheHill_PlayerManager playerManager;

    void Awake()
    {
        playerManager = GetComponent<KingOfTheHill_PlayerManager>();
        InputState.ActivateGameInput();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            AwardExperienceToPlayer(PhotonNetwork.player, 29);
        }
    }

    public override void OnPlayerDeath(PhotonPlayer deadPlayer, PhotonPlayer killer)
    {
        // MAYBE en comptes de passar deadPlayer i killer
        // passar deadPlayer i deathInfo
        playerManager.OnPlayerDeath(deadPlayer);
        PhotonPlayer winner;
        if (CheckEndOfRound(out winner))
        {
            if ( winner != null )
            {
                IncreasePlayerScore(winner);
                AwardExperienceToPlayer(winner, roundWinExperience);
            }
            bool hasGameEnded = CheckEndOfGame();
            if ( winner != null && hasGameEnded)
            {
                AwardExperienceToPlayer(winner, gameWinExperience);
            }

            // end round
            if(hasGameEnded)
            {
                OnRoundEnd();
                OnGameEnd();
            } else
            {
                OnRoundEnd();
                OnRoundSetup();
                OnRoundStart();
            }
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

    private void AwardExperienceToPlayer(PhotonPlayer player, int experience)
    {
        ExitGames.Client.Photon.Hashtable customProperties = player.customProperties;
        customProperties[PlayerProperties.experience] = experience + (int)customProperties[PlayerProperties.experience];
        player.SetCustomProperties(customProperties);
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

    private bool CheckEndOfGame()
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