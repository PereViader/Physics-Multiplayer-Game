using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour, ISetup , IEnd {

    protected virtual void Awake()
    {
        Random.InitState(((int)System.DateTime.Now.Ticks) + ((int)Time.time) + Time.frameCount);
    }

    public virtual void OnGameSetup()
    {
        foreach (ISetup component in GetComponents<ISetup>())
        {

            if ((object)component != this)
                component.OnGameSetup();
        }
    }

    public virtual void OnGameEnd()
    {
        foreach (IEnd component in GetComponents<IEnd>())
        {
            if ((object)component != this)
                component.OnGameEnd();
        }

        if (PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.LoadLevel("EndGameScene");
        }
    }

    public virtual void OnRoundSetup()
    {
        foreach(IGame component in GetComponents<IGame>())
        {
            if((object)component != this)
            {
                component.OnRoundSetup();
            }
        }
    }

    public virtual void OnRoundStart()
    {
        foreach (IGame component in GetComponents<IGame>())
        {
            if ((object)component != this)
            {
                component.OnRoundStart();
            }
        }
    }

    public virtual void OnRoundEnd()
    {
        foreach (IGame component in GetComponents<IGame>())
        {
            if ((object)component != this)
            {
                component.OnRoundEnd();
            }
        }
    }
}
