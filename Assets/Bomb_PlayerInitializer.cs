using UnityEngine;
using System.Collections;

public class Bomb_PlayerInitializer : MonoBehaviour {

    PhotonPlayer player;

    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        player = info.sender;
        player.TagObject = gameObject;
    }

    void OnDestroy()
    {
        player.TagObject = null;
    }
}
