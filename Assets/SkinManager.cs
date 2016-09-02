using UnityEngine;
using System.Collections;

public class SkinManager : MonoBehaviour {

    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        int playerID = (int)GetComponent<PhotonView>().photonView.instantiationData[0];
        string playerTexture = "DefaultMaterial";
        if (PhotonPlayer.Find(playerID).customProperties[PlayerProperties.skin] != null)
        {
            playerTexture = (string)PhotonPlayer.Find(playerID).customProperties[PlayerProperties.skin];
            GetComponent<MeshRenderer>().material = (Material)Resources.Load("PlayerTextures/"+playerTexture);
        }
    }
}
