using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HabilityManager : MonoBehaviour {

    private Hability[] habilities;

    private Text[] habilityName;
    private Text[] habilityCooldown;

    private bool areHabilitiesSet;

	void Awake () {
        CaptureEvents.OnLocalPlayerSpawned += SetPlayer;
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

    void OnDestroy()
    {
        CaptureEvents.OnLocalPlayerSpawned -= SetPlayer;
    }

    void AddRandomHabilities()
    {
        int[] sHabilities = new int[habilities.Length];
        HabilityFabric.FillWithRandomHabilityIndex(ref sHabilities);
        GetComponent<PhotonView>().RPC("RPC_SetHabilities", PhotonTargets.AllBuffered, sHabilities);
    }

    [PunRPC]
    public void RPC_SetHabilities(int[] sHabilities)
    {
        areHabilitiesSet = true;
        for ( int i = 0; i < habilities.Length; i++)
        {
            System.Type habilityType = HabilityFabric.GethabilityType(sHabilities[i]);
            habilities[i] = (Hability)gameObject.AddComponent(habilityType);
            habilities[i].SetVirtualKey("Hability" + (i + 1));
            habilities[i].enabled = false;
        }
    }
    
    void SetPlayer(GameObject player)
    {
        if ( player == gameObject)
        {
            foreach (Hability h in habilities)
                h.enabled = true;
        }
    }    
}
