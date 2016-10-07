using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkinManager : MonoBehaviour {

    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        int playerID = (int)GetComponent<PhotonView>().photonView.instantiationData[0];
        string playerTexture = "DefaultMaterial";
        PhotonPlayer player = PhotonPlayer.Find(playerID);
        if ( player != null )
        {
            if (player.customProperties[PlayerProperties.skin] != null)
            {
                playerTexture = (string)player.customProperties[PlayerProperties.skin];
                GetComponent<MeshRenderer>().material = (Material)Resources.Load("PlayerTextures/" + playerTexture);
            }
            transform.GetComponentInChildren<Text>().text = player.name;
        }
        
    }
}
