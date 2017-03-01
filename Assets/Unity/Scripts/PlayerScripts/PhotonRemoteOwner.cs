using UnityEngine;
using System.Collections;

public class PhotonRemoteOwner : Photon.MonoBehaviour {

    PhotonPlayer player;

    public PhotonPlayer GetPlayer()
    {
        return player;
    }

    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        int playerID = (int)photonView.instantiationData[0];
        player = PhotonPlayer.Find(playerID);
        player.TagObject = gameObject;
    }

    void OnDestroy()
    {
        if ( (GameObject)player.TagObject == gameObject)
            player.TagObject = null;
    }
}
