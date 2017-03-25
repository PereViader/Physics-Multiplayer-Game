using UnityEngine;
using System.Collections;

public class KingOfTheHill_PlayerManager : NewPlayerManager, IPlayerDeath
{
    public override void OnGameSetup()
    {
        if (PhotonNetwork.isMasterClient)
        {
            InitializePlayers();
        }
    }

    public override void OnGameStart() { }

    public override void OnRoundSetup()
    {
        if (PhotonNetwork.isMasterClient)
        {
            SpawnPlayers();
        }
    }

    public override void OnRoundStart() { }

    public override void OnRoundEnd() {
        if (PhotonNetwork.isMasterClient)
        {
            RemovePlayersStillAlive();
        }
    }

    public override void OnGameEnd() { }
    
    protected override void InitializePlayer(PhotonPlayer player)
    {
        ExitGames.Client.Photon.Hashtable basicCustomProperties = new ExitGames.Client.Photon.Hashtable();
        basicCustomProperties["score"] = 0;
        basicCustomProperties[PlayerProperties.experience] = 0;
        player.SetCustomProperties(basicCustomProperties);
    }

    protected void RemovePlayersStillAlive()
    {
        foreach(PhotonPlayer player in PhotonNetwork.playerList)
        {
            if (player.TagObject != null)
                PhotonNetwork.Destroy((GameObject)player.TagObject);
        }
    }

    protected override void SpawnPlayer(PhotonPlayer player)
    {
        ExitGames.Client.Photon.Hashtable customProperties = player.customProperties;
        player.SetCustomProperties(customProperties);

        PhotonNetwork.Instantiate("GameMode/KingOfTheHill/NewPlayer", new Vector3(0, 1, 0), Quaternion.identity, 0, new object[] { player.ID });
        // TODO spawn player in game  at good spawn location  
    }
    
    public override void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        if (PhotonNetwork.isMasterClient)
        {
            OnPlayerDeath(player);
        }
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        if (PhotonNetwork.isMasterClient) // TODO també s'ha d'enviar l'estat del joc actual
        {
            InitializePlayer(player);
        }
    }
}

