using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HabilityManager : Photon.MonoBehaviour {

    [SerializeField]
    int numberOfHabilities;

    Hability[] habilities;

    CaptureUI_HabilityManager habilityManagerUI;

    bool areHabilitiesSet;
    bool isLocalPlayer;

	void Awake () {
        habilityManagerUI = Component.FindObjectOfType<CaptureUI_HabilityManager>();
        habilities = new Hability[numberOfHabilities];
        if (PhotonNetwork.isMasterClient)
        {
            AddRandomHabilities();
        }
    }    

    public void AddRandomHabilities()
    {
        int[] sHabilities = new int[numberOfHabilities];
        HabilityFabric.FillWithRandomHabilityIndex(ref sHabilities);
        photonView.RPC("RPC_SetHabilities", PhotonTargets.AllBufferedViaServer, sHabilities);
    }

    [PunRPC]
    public void RPC_SetHabilities(int[] sHabilities)
    {
        habilityManagerUI.SetActive(true);
        int playerID = (int)photonView.instantiationData[0];
        isLocalPlayer = playerID == PhotonNetwork.player.ID;
            
        for ( int i = 0; i < habilities.Length; i++)
        {
            System.Type habilityType = HabilityFabric.GethabilityType(sHabilities[i]);
            habilities[i] = (Hability)gameObject.AddComponent(habilityType);
            habilities[i].SetVirtualKey("Hability" + (i + 1));
            habilities[i].enabled = isLocalPlayer;
            if ( isLocalPlayer )
            {
                habilityManagerUI.SetName(i, habilities[i].GetHabilityName());
                habilityManagerUI.SetCooldown(i, habilities[i].GetCurrentCooldown());
            }
        }
        areHabilitiesSet = true;
    }

    void OnDestroy()
    {
        if (isLocalPlayer)
            habilityManagerUI.SetActive(false);
    }

    void FixedUpdate()
    {
        if ( isLocalPlayer && areHabilitiesSet )
            for (int i = 0; i < habilities.Length; i++)
                habilityManagerUI.SetCooldown(i, habilities[i].GetCurrentCooldown());
    }
}
