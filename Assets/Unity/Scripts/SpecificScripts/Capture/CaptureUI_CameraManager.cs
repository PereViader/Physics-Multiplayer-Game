using UnityEngine;
using System.Collections;

public class CaptureUI_CameraManager : Photon.MonoBehaviour {

    CameraFollow cameraFollow;
    PlayerFollower playerFollower;

    void Awake()
    {
        cameraFollow = Component.FindObjectOfType<CameraFollow>();
        playerFollower = Component.FindObjectOfType<PlayerFollower>();
    }

	public void SetPlayer(GameObject playerObject)
    {
        PhotonPlayer player = playerObject.GetComponent<PhotonRemoteOwner>().GetPlayer();
        int playerObjectID = playerObject.GetComponent<PhotonView>().viewID;
        photonView.RPC("RPC_SetPlayer", player, playerObjectID);
    }

    [PunRPC]
    void RPC_SetPlayer(int playerViewID)
    {
        GameObject player = PhotonView.Find(playerViewID).gameObject;
        playerFollower.SetPlayer(player);
        cameraFollow.SetObjectToFollow(playerFollower.gameObject);
    }
}
