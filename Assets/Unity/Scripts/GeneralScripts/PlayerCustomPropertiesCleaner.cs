using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCustomPropertiesCleaner : MonoBehaviour {
    [SerializeField]
    bool cleanOnAwake;

    [SerializeField]
    bool cleanOnDestroy;

    void Awake () {
	    if ( cleanOnAwake )
            ClearProperties();
	}

    void OnDestroy()
    {
        if (cleanOnDestroy)
            ClearProperties();
    }
	
	public void ClearProperties()
    {
        ExitGames.Client.Photon.Hashtable clearingHashtable = new ExitGames.Client.Photon.Hashtable();

        var keys = PhotonNetwork.player.customProperties.Keys;
        foreach (var key in keys)
        {
            clearingHashtable.Add(key, null);
        }
        PhotonNetwork.player.SetCustomProperties(clearingHashtable);
    }
}
