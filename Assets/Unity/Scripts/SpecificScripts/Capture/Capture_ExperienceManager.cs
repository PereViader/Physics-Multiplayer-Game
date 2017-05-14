using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Capture_ExperienceManager : ExperienceManager {
    [System.Serializable]
    public class CaptureExperience {
        public int score;
        public int kill;
        public int winGame;
    }

    [SerializeField]
    public CaptureExperience experienceValues;

    public void AddExperienceToTeam(int team, int amount)
    {
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            int playerTeam = (int)player.customProperties[PlayerProperties.team];
            if (playerTeam == team)
            {
                AddExperience(player, amount);
            }
        }
    }
}
