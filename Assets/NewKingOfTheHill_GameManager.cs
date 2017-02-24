using UnityEngine;
using System.Collections;

public class NewKingOfTheHill_GameManager : MonoBehaviour {

    GameEventManager gameEventManager;

    void Awake()
    {
        gameEventManager = GetComponent<GameEventManager>();
    }

    public virtual void OnNetworkReady()
    {
        TriggerStartingGameEvents();
    }

    public virtual void TriggerStartingGameEvents()
    {
        gameEventManager.OnGameSetup();
        gameEventManager.OnGameStart();
        gameEventManager.OnRoundSetup();
        gameEventManager.OnRoundStart();
    }


}
