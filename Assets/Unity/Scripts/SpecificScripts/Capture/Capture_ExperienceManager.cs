using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Capture_ExperienceManager : Photon.MonoBehaviour, IGame {
    [System.Serializable]
    public class ExperienceType {
        public int score;
        public int kill;
        public int winGame;
        public int joinGameAlreadyStarted;
    }

    [SerializeField]
    public ExperienceType experienceValues;

    public void OnGameSetup()
    {
        foreach (var player in PhotonNetwork.playerList)
            InitializePlayer(player);
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

    void InitializePlayer(PhotonPlayer player)
    {
        player.customProperties[PlayerProperties.experience] = 0;
        player.SetCustomProperties(player.customProperties);
    }

    public void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        if ( PhotonNetwork.isMasterClient )
        {
            InitializePlayer(player);
            AddExperience(player, experienceValues.joinGameAlreadyStarted);
        }
    }

    public void AddExperience(PhotonPlayer player, int amount)
    {
        ExitGames.Client.Photon.Hashtable customProperties = player.customProperties;
        customProperties[PlayerProperties.experience] = amount + (int)customProperties[PlayerProperties.experience];
        player.SetCustomProperties(customProperties);
    }

    public void AddExperienceToTeam(int team, int amount)
    {
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            int playerTeam = (int)player.customProperties[PlayerProperties.team];
            if (playerTeam == team)
            {
                AddExperience(player, amount);
            }
        }
    }
}
