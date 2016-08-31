using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Bomb_EndGameManager : Photon.MonoBehaviour {

    [SerializeField]
    int gameDurationInSeconds;

    float timeUntilEnd;
    TimeManagerUI timeMangerUI;

    void Awake()
    {
        Bomb_Events.OnEndGame += OnEndGame;
        timeUntilEnd = gameDurationInSeconds;
        Text minutes, seconds;
        minutes = GameObject.Find("UI_Clock/Minutes").GetComponent<Text>();
        seconds = GameObject.Find("UI_Clock/Seconds").GetComponent<Text>();
        timeMangerUI = new TimeManagerUI(minutes, seconds);
    }

    void OnDestroy()
    {
        Bomb_Events.OnEndGame -= OnEndGame;
    }

    void FixedUpdate()
    {
        if ( PhotonNetwork.isMasterClient )
            if ( timeUntilEnd > 0)
            {
                timeUntilEnd = Mathf.Clamp(timeUntilEnd-Time.fixedDeltaTime,0,float.MaxValue);
                timeMangerUI.UpdateTime((int)timeUntilEnd);
            } else
            {
                Bomb_Events.CallOnEndGame();
            }
    }

    void OnEndGame()
    {
        enabled = false;
        if ( PhotonNetwork.isMasterClient )
        {
            Dictionary<PhotonPlayer, int> playerExperience = GetComponent<Bomb_ExperienceManager>().GetExperienceOfAllPlayers();
            foreach (var player in playerExperience)
                photonView.RPC("Rpc_EndGame", player.Key, player.Value);
        }
    }

    [PunRPC]
    void Rpc_EndGame(int experience)
    {

    }
}
