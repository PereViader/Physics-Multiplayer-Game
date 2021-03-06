﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class CaptureUI_ScoreManager : MonoBehaviour {

    [SerializeField]
    int teamsInGame;

    Text[] scoreUI;

    void Awake()
    {
        scoreUI = new Text[teamsInGame];
        for ( int i = 0; i<teamsInGame; i++)
        {
            scoreUI[i] = GameObject.Find("Canvas/GameUI/Marcador/Score"+i).GetComponent<Text>();
            scoreUI[i].text = "0";
        }
    }

    void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        for (int team = 0; team < teamsInGame; team++)
        {
            if (propertiesThatChanged.ContainsKey(RoomProperties.Score + team))
            {
                int score = (int)propertiesThatChanged[RoomProperties.Score + team];
                scoreUI[team].text = score.ToString();
            }
        }
    }

    public void SetScore(int[] score)
    {
        if (score.Length != teamsInGame)
            Debug.Log("ScoreUI " + scoreUI.Length + " not equal to provided score " + score.Length);
        else
        {
            for ( int i = 0; i < teamsInGame; i++)
            {
                scoreUI[i].text = score[i].ToString();
            }
        }
    }

    public void SetScore(int team, int score)
    {
        scoreUI[team].text = score.ToString();
    }

    public void Score(int team, int value = 1)
    {
        int score = int.Parse(scoreUI[team].text);
        scoreUI[team].text = (score + 1).ToString();
    }
}
