﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public abstract class GameEventManager : MonoBehaviour, IGame
{
    public virtual void TriggerStartingGameEvents()
    {
        OnGameSetup();
        OnGameStart();
        OnRoundSetup();
        OnRoundStart();
    }

    public virtual void OnGameEnd()
    {
        foreach(IGame component in GetComponents<IGame>() )
        {
            if( (object) component != this )
                component.OnGameEnd();
        }
        GetComponent<PhotonView>().RPC("RPC_EndGame", PhotonTargets.All);
    }

    public virtual void OnGameSetup()
    {
        foreach (IGame component in GetComponents<IGame>())
        {
            if( (object) component != this )
                component.OnGameSetup();
        }
    }

    public virtual void OnGameStart()
    {
        foreach (IGame component in GetComponents<IGame>())
        {
            if( (object) component != this )
                component.OnGameStart();
        }
    }

    public virtual void OnRoundEnd()
    {
        foreach (IGame component in GetComponents<IGame>())
        {
            if( (object) component != this )
                component.OnRoundEnd();
        }
    }

    public virtual void OnRoundSetup()
    {
        foreach (IGame component in GetComponents<IGame>())
        {
            if( (object) component != this )
                component.OnRoundSetup();
        }
    }

    public virtual void OnRoundStart()
    {
        foreach (IGame component in GetComponents<IGame>())
        {
            if( (object) component != this )
                component.OnRoundStart();
        }
    }

    [PunRPC]
    public virtual void RPC_EndGame()
    {
        EndGameManager.experienceGained = (int)PhotonNetwork.player.customProperties[PlayerProperties.experience];
        EndGameManager.gameResult = GetGameResultForPlayer(PhotonNetwork.player);
        SceneManager.LoadScene("EndGameScene");
    }

    public abstract PlayerProperties.GameResult GetGameResultForPlayer(PhotonPlayer player);
}
