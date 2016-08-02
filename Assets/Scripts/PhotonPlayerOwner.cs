using UnityEngine;
using System.Collections;

public class PhotonPlayerOwner : MonoBehaviour {

    int photonPlayerId;
    PhotonPlayer player;
    int pos;
    [SerializeField]
    int team; 

    public void SetOwner(int id)
    {
        Debug.Log("id"+id);
        photonPlayerId = id;
        player = PhotonPlayer.Find(id);
        if (player == null)
            Debug.Log("Set owner did not work");
        pos = Random.Range(0, 6);
    }



    public int GetOwnerId()
    {
        return photonPlayerId;
    }

    public PhotonPlayer GetOwner()
    {
        return player;
    }

    void OnGUI()
    {
        if ( player != null)
        {
            team = (int)player.customProperties["Team"];
            GUI.Box(new Rect(350 + 60 * pos, 40, 60, 25), "Team: " +team);

        }

    }
}
