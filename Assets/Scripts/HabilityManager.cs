﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HabilityManager : MonoBehaviour {

    private Hability[] habilities;

    private Text[] habilityName;
    private Text[] habilityCooldown;

	void Awake () {
        habilities = new Hability[2];
        habilityName = new Text[2];
        habilityCooldown = new Text[2];
        GameObject habilitiesUI = GameObject.Find("Canvas").transform.Find("HabilitiesUI").gameObject;
        for ( int i = 0; i < habilities.Length; i++)
        {
            GameObject habilityUI = habilitiesUI.transform.GetChild(i).gameObject;
            habilityName[i] = habilityUI.transform.Find("Name").GetComponent<Text>();
            habilityCooldown[i] = habilityUI.transform.Find("Cooldown").GetComponent<Text>();
        }
        
        if (PhotonNetwork.isMasterClient)
            AddRandomHabilities();

    }

    void Update()
    {
        for (int i = 0; i < habilities.Length; i++)
        {
            habilityCooldown[i].text = (habilities[i].GetCurrentCooldown()).ToString("0.0");
        }
    }

    void AddRandomHabilities()
    {
        int randomHability1 = Random.Range(0, 3);
        int randomHability2;
        do
        {
            randomHability2 = Random.Range(0, 3);
        } while (randomHability2 == randomHability1);

        GetComponent<PhotonView>().RPC("NetworkAddRandomHability", PhotonTargets.AllBuffered, randomHability1,randomHability2);
    }

    [PunRPC]
    public void NetworkAddRandomHability(int hability1Index, int hability2Index)
    {
        habilities[0] = AddHability("Hability" + 1, hability1Index);
        habilities[1] = AddHability("Hability" + 2, hability2Index);
        for (int i = 0; i<habilities.Length; i++)
        {
            habilityName[i].text = habilities[i].GetHabilityName();
        }
    }

    Hability AddHability (string habilityVirtualKey, int habilityNumber)
    {
        Hability hability; 
        switch (habilityNumber)
        {
            case 0:
                hability = gameObject.AddComponent<HabilityJump>();
                break;
            case 1:
                hability = gameObject.AddComponent<HabilityGuard>();
                break;
            case 2:
            default:
                hability = gameObject.AddComponent<HabilityPush>();
                break;
        }
        hability.SetVirtualKey(habilityVirtualKey);
        hability.enabled = false;
        return hability;
    }

    public void ActivateInputCaptureForHabilities()
    {
        foreach ( Hability hability in habilities)
        {
            hability.enabled = true;
        }
    }
}
