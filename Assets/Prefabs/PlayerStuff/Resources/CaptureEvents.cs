using UnityEngine;
using System.Collections;

public class CaptureEvents {

    public delegate void GameObjectDelegate (GameObject gameObject);
    public delegate void IntegerDelegate(int integer);
    //public delegate void EmptyDelegate();

    public static event GameObjectDelegate OnLocalPlayerSpawned;
    public static event GameObjectDelegate OnCaptureZoneCreated;

    public static event IntegerDelegate OnTeamScored;

    //public static event EmptyDelegate OnInputCaptureChange;

    public static void CallOnCaptureZoneCreated(GameObject captureZone)
    {
        if ( OnCaptureZoneCreated != null )
            OnCaptureZoneCreated(captureZone);
    }

    public static void CallOnLocalPlayerSpawned(GameObject player)
    {
        if (OnLocalPlayerSpawned != null)
            OnLocalPlayerSpawned(player);
    }

    public static void CallOnTeamScored(int team)
    {
        if (OnTeamScored != null)
            OnTeamScored(team);
    }


}
