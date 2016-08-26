using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HabilityManager : Photon.MonoBehaviour {

    [SerializeField]
    int numberOfHabilities;

    Hability[] habilities;

	void Awake () {
        CaptureEvents.OnLocalPlayerSpawned += OnLocalPlayerSpawned;
        habilities = new Hability[numberOfHabilities];
    }    

    void OnDestroy()
    {
        CaptureEvents.OnLocalPlayerSpawned -= OnLocalPlayerSpawned;
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
        for ( int i = 0; i < habilities.Length; i++)
        {
            System.Type habilityType = HabilityFabric.GethabilityType(sHabilities[i]);
            habilities[i] = (Hability)gameObject.AddComponent(habilityType);
            habilities[i].SetVirtualKey("Hability" + (i + 1));
            habilities[i].enabled = false;
        }
    }

    void OnLocalPlayerSpawned(GameObject player)
    {
        if ( player == gameObject)
        {
            EnableHabilities();
        }
    }

    void EnableHabilities()
    {
        foreach (Hability h in habilities)
            h.enabled = true;
    }
}
