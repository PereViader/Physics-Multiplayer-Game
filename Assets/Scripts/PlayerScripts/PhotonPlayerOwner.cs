using UnityEngine;
using System.Collections;

public class PhotonPlayerOwner : MonoBehaviour {

    int photonPlayerId;
    PhotonPlayer player;
    int team;

    public delegate void EmptyDelegate();
    public event EmptyDelegate OnPlayerOwnerSet;

    [PunRPC]
    public void SetOwner(int id)
    {
        photonPlayerId = id;
        player = PhotonPlayer.Find(id);
        if (player == null)
            Debug.LogWarning("Set owner did not work");
        team = (int)player.customProperties["Team"];

        if ( player == PhotonNetwork.player )
        {
            CaptureEvents.CallOnLocalPlayerSpawned(gameObject);
        }
        CallOnPlayerOwnerSet();
    }

    void CallOnPlayerOwnerSet()
    {
        if (OnPlayerOwnerSet != null)
            OnPlayerOwnerSet();
    }

    public int GetOwnerId()
    {
        return photonPlayerId;
    }

    public PhotonPlayer GetOwner()
    {
        return player;
    }

    public int GetTeam()
    {
        return team;
    }

}
