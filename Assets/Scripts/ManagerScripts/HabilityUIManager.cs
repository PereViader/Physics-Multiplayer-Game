﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HabilityUIManager : MonoBehaviour {

    GameObject player;
    GameObject habilityUI;
    Hability[] playerHability;
    Text[] habilityNameUI;
    Text[] habilityCooldownUI;

    GameObject HabilityUI;

    [SerializeField]
    int numberOfHabilities;

    void Awake()
    {
        CaptureEvents.OnLocalPlayerSpawned += OnPlayerSpawned;
        CaptureEvents.OnLocalPlayerKilled += OnPlayerKilled;

        habilityNameUI = new Text[numberOfHabilities];
        habilityCooldownUI = new Text[numberOfHabilities];
        habilityUI = GameObject.Find("Canvas").transform.Find("HabilitiesUI").gameObject;
        for (int i = 0; i < numberOfHabilities; i++)
        {
            Transform currentHability = habilityUI.transform.GetChild(i);
            habilityNameUI[i] = currentHability.Find("Name").GetComponent<Text>();
            habilityCooldownUI[i] = currentHability.Find("Cooldown").GetComponent<Text>();
        }
        
    }

    void OnDestroy()
    {
        CaptureEvents.OnLocalPlayerSpawned -= OnPlayerSpawned;
        CaptureEvents.OnLocalPlayerKilled -= OnPlayerKilled;
    }

    void Update()
    {
        if ( player != null)
        {
            UpdateHabilityCooldownUI();
        }
    }

    void UpdateHabilityCooldownUI()
    {
        for (int i = 0; i < playerHability.Length; i++)
        {
            habilityCooldownUI[i].text = playerHability[i].GetCurrentCooldown().ToString("0.0");
        }
    }

    void UpdateHabilityNameUI()
    {
        for (int i = 0; i < playerHability.Length; i++)
        {
            habilityNameUI[i].text = playerHability[i].GetHabilityName();
        }
    }

    void OnPlayerSpawned(GameObject player)
    {
        this.player = player;
        habilityUI.SetActive(true);
        playerHability = player.GetComponents<Hability>();
        if (playerHability.Length != numberOfHabilities)
            Debug.Log("Habilities and numberOfHabilities don't match\nPlayerHabilities "+ playerHability.Length+"\nnumberOfHabilities "+numberOfHabilities);
        UpdateHabilityNameUI();
    }

    void OnPlayerKilled(GameObject player)
    {
        if ( this.player == player)
        {
            player = null;
            habilityUI.SetActive(false);
        }
    }
}