using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HabilityManager : Photon.MonoBehaviour {

    [SerializeField]
    private int numberOfHabilities;

    private Hability[] habilities;

    private bool isLocalPlayer;

    private UI_HabilityManager habilityManagerUI;


    void Awake () {
        habilityManagerUI = GameObject.FindObjectOfType<UI_HabilityManager>();
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
        habilityManagerUI.setDisplay(true);

        int playerID = (int)photonView.instantiationData[0];
        isLocalPlayer = playerID == PhotonNetwork.player.ID;
        for ( int i = 0; i < habilities.Length; i++)
        {
            System.Type habilityType = HabilityFabric.GethabilityType(sHabilities[i]);
            habilities[i] = (Hability)gameObject.AddComponent(habilityType);
            habilities[i].enabled = isLocalPlayer;
            if (isLocalPlayer)
                habilities[i].Initialize(habilityManagerUI.getHability(i), i);
        }
    }

    void OnDestroy()
    {
        try
        {
            if (isLocalPlayer)
                habilityManagerUI.setDisplay(false);
        }
        catch { }
    }
}
