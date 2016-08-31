using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Capture_ExperienceManager : Photon.MonoBehaviour {
    [System.Serializable]
    public class ExperienceType {
        public int score;
        public int kill;
        public int winGame;
        public int joinGameAlreadyStarted;
    }

    [SerializeField]
    public ExperienceType experienceValues;

    Dictionary<int, int> experience;
    CaptureUI_ExperienceManager experienceManagerUI;

    void Awake()
    {
        experience = new Dictionary<int, int>();
        experienceManagerUI = Component.FindObjectOfType<CaptureUI_ExperienceManager>();
    }

    public void OnGameModeSetup()
    {
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
            InitializePlayer(player);
    }

    public void OnGameModeEnded()
    {
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
            player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { PlayerProperties.experience, experience[player.ID] } });
    }

    public void InitializePlayer(PhotonPlayer player)
    {
        experience.Add(player.ID, 0);
    }

    public void AddExperience(PhotonPlayer player, int amount)
    {
        photonView.RPC("RPC_AddExperience", PhotonTargets.All, player.ID, amount);
    }

    [PunRPC]
    void RPC_AddExperience(int playerID, int amount )
    {
        experience[playerID] += amount;
        if ( PhotonNetwork.player.ID == playerID)
            experienceManagerUI.DisplayAddedExperience(amount);
    }

    public void AddExperienceToTeam(int team, int amount)
    {
        photonView.RPC("RPC_AddExperienceToTeam", PhotonTargets.All, team, amount);
    }

    [PunRPC]
    public void RPC_AddExperienceToTeam(int team, int amount)
    {
        foreach( PhotonPlayer player in PhotonNetwork.playerList)
        {
            int playerTeam = (int)player.customProperties[PlayerProperties.team];
            if ( playerTeam == team )
            {
                experience[player.ID] += amount;
                if (PhotonNetwork.player == player)
                    experienceManagerUI.DisplayAddedExperience(amount);
            }
        }
    }

    void OnGUI()
    {
        try
        {
            GUI.Box(new Rect(0, 70, 150, 30), "Experience: " + experience[PhotonNetwork.player.ID]);
        }
        catch(System.Exception)
        {

        }
    }
}
