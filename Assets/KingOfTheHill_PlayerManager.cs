using UnityEngine;
using System.Collections;
using System;

public class KingOfTheHill_PlayerManager : PlayerManager
{
    public override bool IsFriendly(GameObject gameObject1, GameObject gameObject2)
    {
        return gameObject1 == gameObject2;
    }

    public override void OnGameEnd()
    {
    }

    protected override void InitializePlayer(PhotonPlayer player)
    {
        ExitGames.Client.Photon.Hashtable basicCustomProperties = new ExitGames.Client.Photon.Hashtable();
        basicCustomProperties["score"] = 0;
        basicCustomProperties["experience"] = 0;
        player.SetCustomProperties(basicCustomProperties);
    }

    protected override void SpawnPlayer(PhotonPlayer player)
    {
        ExitGames.Client.Photon.Hashtable customProperties = player.customProperties;
        customProperties["isAlive"] = true;
        player.SetCustomProperties(customProperties);

        PhotonNetwork.Instantiate("GameMode/KingOfTheHill/NewPlayer", new Vector3(0, 10, 2), Quaternion.identity, 0, new object[] { player.ID });
        // TODO spawn player in game    
    }
}
