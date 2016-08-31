using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HabilityManager : Photon.MonoBehaviour {

    [SerializeField]
    int numberOfHabilities;

    Hability[] habilities;

    CaptureUI_HabilityManager habilityManager;
    PhotonRemoteOwner remoteOwner;

    bool areHabilitiesSet;

	void Awake () {
        remoteOwner = GetComponent<PhotonRemoteOwner>();
        habilityManager = Component.FindObjectOfType<CaptureUI_HabilityManager>();
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
        habilityManager.SetActive(true);
        int playerID = (int)photonView.instantiationData[0];
        bool isLocalPlayer = playerID == PhotonNetwork.player.ID;
            
        for ( int i = 0; i < habilities.Length; i++)
        {
            System.Type habilityType = HabilityFabric.GethabilityType(sHabilities[i]);
            habilities[i] = (Hability)gameObject.AddComponent(habilityType);
            habilities[i].SetVirtualKey("Hability" + (i + 1));
            habilities[i].enabled = isLocalPlayer;
            habilityManager.SetName(i, habilities[i].GetHabilityName());
            habilityManager.SetCooldown(i, 0);
        }
        areHabilitiesSet = true;
    }

    void OnDestroy()
    {
        if (remoteOwner.GetPlayer() == PhotonNetwork.player)
            habilityManager.SetActive(false);
    }

    void FixedUpdate()
    {
        if ( areHabilitiesSet)
            for (int i = 0; i < habilities.Length; i++)
                habilityManager.SetCooldown(i, habilities[i].GetCurrentCooldown());
    }
}
