using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bomb_ExperienceManager : Photon.MonoBehaviour {

    Dictionary<PhotonPlayer, int> playerExperience;

    void Awake()
    {
        playerExperience = new Dictionary<PhotonPlayer, int>();
        AddStartingPlayers();
    }

    void AddStartingPlayers()
    {
        foreach (PhotonPlayer photonPlayer in PhotonNetwork.playerList)
            AddPlayer(photonPlayer);
    }

    void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        AddPlayer(newPlayer);
    }

    void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        RemovePlayer(otherPlayer);
    }

    void AddPlayer(PhotonPlayer photonPlayer)
    {
        playerExperience.Add(photonPlayer, 0);
    }

    void RemovePlayer(PhotonPlayer photonPlayer)
    {
        playerExperience.Remove(photonPlayer);

    }

    void AddExperience(PhotonPlayer player, int experience)
    {
        playerExperience[player] += experience;
    }

    public Dictionary<PhotonPlayer,int> GetExperienceOfAllPlayers()
    {
        return playerExperience;
    }
}
