using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public abstract class GameManager : Photon.MonoBehaviour, IGame
{
    public virtual void OnGameSetup()
    {
        foreach (IGame component in GetComponents<IGame>())
        {
            if( (object) component != this )
                component.OnGameSetup();
        }
        OnGameStart();
    }

    public virtual void OnGameStart()
    {
        foreach (IGame component in GetComponents<IGame>())
        {
            if( (object) component != this )
                component.OnGameStart();
        }
        OnRoundSetup();
    }

    public virtual void OnRoundSetup()
    {
        foreach (IGame component in GetComponents<IGame>())
        {
            if ((object)component != this)
                component.OnRoundSetup();
        }
        OnRoundStart();
    }

    public virtual void OnRoundStart()
    {
        foreach (IGame component in GetComponents<IGame>())
        {
            if ((object)component != this)
                component.OnRoundStart();
        }
    }

    public virtual void OnRoundEnd()
    {
        foreach (IGame component in GetComponents<IGame>())
        {
            if ((object)component != this)
                component.OnRoundEnd();
        }
        if (HasGameEnded())
            OnGameEnd();
        else
            OnRoundSetup();
    }

    public virtual void OnGameEnd()
    {
        foreach(IGame component in GetComponents<IGame>() )
        {
            if( (object) component != this )
                component.OnGameEnd();
        }
        photonView.RPC("RPC_EndGame", PhotonTargets.All);
    }

    [PunRPC]
    public virtual void RPC_EndGame()
    {
        EndGameManager.experienceGained = (int)PhotonNetwork.player.customProperties[PlayerProperties.experience];
        EndGameManager.gameResult = GetGameResultForPlayer(PhotonNetwork.player);
        SceneManager.LoadScene("EndGameScene");
    }

    public abstract bool HasGameEnded();

    public abstract void OnPlayerDeath(PhotonPlayer deadPlayer, PhotonPlayer killer);

    public abstract PlayerProperties.GameResult GetGameResultForPlayer(PhotonPlayer player);
}