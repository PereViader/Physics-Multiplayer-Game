using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HabilityManager : MonoBehaviour {

    private Hability[] habilities;

    private Text[] habilityName;
    private Text[] habilityCooldown;

    private bool areHabilitiesSet;

	void Awake () {
        habilities = new Hability[2];
        habilityName = new Text[2];
        habilityCooldown = new Text[2];
        Transform habilitiesUI = GameObject.Find("Canvas").transform.Find("HabilitiesUI");
        for ( int i = 0; i < habilities.Length; i++)
        {
            Transform habilityUI = habilitiesUI.GetChild(i);
            habilityName[i] = habilityUI.Find("Name").GetComponent<Text>();
            habilityCooldown[i] = habilityUI.Find("Cooldown").GetComponent<Text>();
        }
        
        if (PhotonNetwork.isMasterClient)
            AddRandomHabilities();
        
    }

    void Update()
    {
        if (areHabilitiesSet)
            for (int i = 0; i < habilities.Length; i++)
            {
                habilityCooldown[i].text = (habilities[i].GetCurrentCooldown()).ToString("0.0");
            }
    }

    int GetRandomHabilityID()
    {
        return Random.Range(0, 5);
    }

    void AddRandomHabilities()
    {
        int randomHability1 = GetRandomHabilityID();
        int randomHability2;
        do{
            randomHability2 = GetRandomHabilityID();
        } while (randomHability2 == randomHability1);

        GetComponent<PhotonView>().RPC("RPC_SetHabilities", PhotonTargets.AllBuffered, randomHability1,randomHability2);
    }

    [PunRPC]
    public void RPC_SetHabilities(int hability1Index, int hability2Index)
    {
        areHabilitiesSet = true;
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
                hability = gameObject.AddComponent<HabilityPush>();
                break;
            case 3:
                hability = gameObject.AddComponent<HabilityShrink>();
                break;
            case 4:
            default:
                hability = gameObject.AddComponent<HabilityStop>();
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
