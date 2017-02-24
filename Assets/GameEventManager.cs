using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class GameEventManager : MonoBehaviour, IGame
{
    

    public virtual void OnGameEnd()
    {
        foreach(IGame component in GetComponents<IGame>() )
        {
            if( (object) component != this )
                component.OnGameEnd();
        }
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
}
