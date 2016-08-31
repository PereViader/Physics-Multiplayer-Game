using UnityEngine;
using System.Collections;

public class Capture_Initializer : Photon.MonoBehaviour {

    int playerTeam;
    PhotonPlayer player;

    public PhotonPlayer GetPlayer()
    {
        return player;
    }

    public int GetTeam()
    {
        return playerTeam;
    }

    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        int playerID = (int)photonView.instantiationData[0];
        player = PhotonPlayer.Find(playerID);
        player.TagObject = gameObject;
        playerTeam = (int)player.customProperties["Team"];
        if ( playerID == PhotonNetwork.player.ID)
        {
            CaptureEvents.CallOnLocalPlayerSpawned(gameObject);
        }
    }

    void OnDestroy()
    {
        player.TagObject = null;
    }
}
