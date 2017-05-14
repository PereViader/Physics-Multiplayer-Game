using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class ExperienceManager : Photon.MonoBehaviour, IGame
{
    [SerializeField]
    private int joinGameAlreadyStarted;

    public virtual void OnGameSetup()
    {
        foreach (var player in PhotonNetwork.playerList)
            InitializePlayer(player);
    }

    public virtual void OnGameStart()
    {
    }

    public virtual void OnRoundSetup()
    {
    }

    public virtual void OnRoundStart()
    {
    }

    public virtual void OnRoundEnd()
    {
    }

    public virtual void OnGameEnd()
    {
    }

    protected virtual void InitializePlayer(PhotonPlayer player)
    {
        player.customProperties[PlayerProperties.experience] = 0;
        player.SetCustomProperties(player.customProperties);
    }

    public virtual void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        if (PhotonNetwork.isMasterClient)
        {
            InitializePlayer(player);
            AddExperience(player, joinGameAlreadyStarted);
        }
    }

    public void AddExperience(PhotonPlayer player, int amount)
    {
        ExitGames.Client.Photon.Hashtable customProperties = player.customProperties;
        customProperties[PlayerProperties.experience] = amount + (int)customProperties[PlayerProperties.experience];
        player.SetCustomProperties(customProperties);
    }
}
