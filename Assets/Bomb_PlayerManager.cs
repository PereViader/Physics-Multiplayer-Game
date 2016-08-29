using UnityEngine;
using System.Collections;

public class Bomb_PlayerManager : MonoBehaviour {

    void Awake()
    {
        InstantiateLocalPlayer();
    }

    void InstantiateLocalPlayer()
    {
        GameObject localPlayer = (GameObject)PhotonNetwork.Instantiate("BombPlayer", Vector3.zero, Quaternion.identity, 0);
        if (!PhotonNetwork.isMasterClient)
        {
            localPlayer.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.masterClient);
        }
    }
}
