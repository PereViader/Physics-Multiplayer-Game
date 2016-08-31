using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerScoreManager : Photon.MonoBehaviour {
    [SerializeField]
    int scorePoints;

    [SerializeField]
    int captureHelpScore;

    [SerializeField]
    int killScore;

    [SerializeField]
    int winGame;

    [SerializeField]
    int lateJoin;

    void Awake()
    {
        CaptureEvents.OnPlayerKilled += OnPlayerKilled;
        CaptureEvents.OnGameEnded += OnMatchEnded;
        CaptureEvents.OnTeamScored += OnTeamCaptured;
        InitializeExperience();
    }

    void InitializeExperience()
    {
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
            InitializeExperience(player);
    }

    void InitializeExperience(PhotonPlayer player)
    {
        player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "Experience", 0 } });
    }

    void OnDestroy()
    {
        CaptureEvents.OnPlayerKilled -= OnPlayerKilled;
        CaptureEvents.OnGameEnded -= OnMatchEnded;
        CaptureEvents.OnTeamScored -= OnTeamCaptured;
    }

    void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        InitializeExperience(player);
        IncreaseExperienceToPlayer(player, lateJoin);
    }

    void OnPlayerKilled(PhotonPlayer killer, PhotonPlayer killed)
    {
        IncreaseExperienceToPlayer(killer, killScore);
    }

    void OnTeamCaptured(int team)
    {
        IncreaseExperienceToTeamPlayers(team, scorePoints);
    }

    void IncreaseExperienceToTeamPlayers(int team, int experience)
    {
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            int playerTeam = (int)player.customProperties["Team"];
            Debug.Log("playerteam " + playerTeam + " team " + team);
            if (playerTeam == team)
            {
                IncreaseExperienceToPlayer(player, experience);
            }
        }
    }

    void IncreaseExperienceToPlayer(PhotonPlayer player, int experience)
    {
        Debug.Log("added experience to player" + player.ID + " amount: " + experience);
        if ( experience != 0 )
        {
            int currentExperience = (int)player.customProperties["Experience"];
            int newExperience = currentExperience + experience;
            Debug.Log("Player " + player.ID + " now has " + newExperience + " experience");
            player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "Experience", newExperience } });
        }
    }

    [PunRPC]
    void RPC_NotifyExperienceChanged(int experience)
    {

    }


    void OnMatchEnded(int team)
    {
        Debug.Log("Match ended" + team);
        IncreaseExperienceToTeamPlayers(team, winGame);
    }
}
