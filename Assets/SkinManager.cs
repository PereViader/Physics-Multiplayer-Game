using UnityEngine;
using System.Collections;

public class SkinManager : Photon.MonoBehaviour {

    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        int playerID = (int)photonView.instantiationData[0];
        string playerSkin = "DefaultMaterial";
        if (PhotonPlayer.Find(playerID).customProperties[PlayerProperties.skin] != null)
        {
            playerSkin = (string)PhotonPlayer.Find(playerID).customProperties["Skin"];
            GetComponent<MeshRenderer>().material = (Material)Resources.Load(playerSkin);
        }
    }
}
