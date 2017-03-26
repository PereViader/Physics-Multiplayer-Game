using UnityEngine;
using System.Collections;

public class CameraTakeOver : MonoBehaviour {

    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        bool isLocalPlayer = PhotonNetwork.player.ID == (int)GetComponent<PhotonView>().instantiationData[0];
        if (isLocalPlayer)
        {
            GameObject.FindObjectOfType<CameraFollow>().SetObjectToFollow(gameObject);
        }
    }
}
