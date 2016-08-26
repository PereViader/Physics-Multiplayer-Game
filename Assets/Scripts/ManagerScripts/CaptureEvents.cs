﻿using UnityEngine;
using System.Collections;

public class CaptureEvents {

    public delegate void GameObjectDelegate (GameObject gameObject);
    public delegate void IntegerDelegate(int integer);
    public delegate void PhotonPlayerDelegate(PhotonPlayer photonPlayer);

    public delegate void EmptyDelegate();

    public static event GameObjectDelegate OnLocalPlayerSpawned;
    public static event GameObjectDelegate OnLocalPlayerKilled;
    public static event GameObjectDelegate OnCaptureZoneCreated;
    public static event GameObjectDelegate OnCaptureZoneDestroyed;

    public static event PhotonPlayerDelegate OnPlayerKilled;

    public static event EmptyDelegate OnJoinedStartedGame;

    public static event IntegerDelegate OnTeamScored;
    public static event IntegerDelegate OnGameEnded;

    //public static event EmptyDelegate OnInputCaptureChange;

    public static void CallOnPlayerKilled(PhotonPlayer killer)
    {
        if (OnPlayerKilled != null)
            OnPlayerKilled(killer);
    }

    public static void CallOnCaptureZoneCreated(GameObject captureZone)
    {
        if ( OnCaptureZoneCreated != null )
            OnCaptureZoneCreated(captureZone);
    }

    public static void CallOnCaptureZoneDestroyed(GameObject captureZone)
    {
        if (OnCaptureZoneDestroyed != null)
            OnCaptureZoneDestroyed(captureZone);
    }

    public static void CallOnLocalPlayerSpawned(GameObject player)
    {
        if (OnLocalPlayerSpawned != null)
            OnLocalPlayerSpawned(player);
    }

    public static void CallOnLocalPlayerKilled(GameObject player)
    {
        if (OnLocalPlayerKilled != null)
            OnLocalPlayerKilled(player);
    }

    public static void CallOnTeamScored(int team)
    {
        if (OnTeamScored != null)
            OnTeamScored(team);
    }

    public static void CallOnGameEnded(int team)
    { 
        if (OnGameEnded != null)
        {
            OnGameEnded(team);
        }
    }


}