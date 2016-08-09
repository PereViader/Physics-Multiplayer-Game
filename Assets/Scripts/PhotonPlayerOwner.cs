using UnityEngine;
using System.Collections;

public class PhotonPlayerOwner : MonoBehaviour {

    int photonPlayerId;
    PhotonPlayer player;
    int team;

    bool hasBeenSet = false;

    [PunRPC]
    public void SetOwner(int id)
    {
        hasBeenSet = true;
        photonPlayerId = id;
        player = PhotonPlayer.Find(id);
        if (player == null)
            Debug.LogWarning("Set owner did not work");
    }

    public bool HasBeenSet()
    {
        return hasBeenSet;
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
