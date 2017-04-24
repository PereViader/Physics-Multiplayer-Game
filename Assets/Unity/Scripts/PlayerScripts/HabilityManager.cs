using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class HabilityManager : Photon.MonoBehaviour {

    [SerializeField]
    private int numberOfHabilities;

    private Hability[] habilities;

    void Awake () {
        if (PhotonNetwork.isMasterClient)
        {
            AddRandomHabilities();
        }
    }

    public void AddRandomHabilities()
    {
        int seed =  Mathf.FloorToInt(UnityEngine.Random.Range(Int32.MinValue, Int32.MaxValue));
        photonView.RPC("RPC_AddRandomHabilities", PhotonTargets.AllBufferedViaServer, seed);
    }

    [PunRPC]
    public void RPC_AddRandomHabilities(int seed)
    {
        Type[] tHabilities = HabilityFabric.GenerateHabilitiesFromSeed(seed, numberOfHabilities);
        int playerID = (int)photonView.instantiationData[0];
        bool isLocalPlayer = playerID == PhotonNetwork.player.ID;
        habilities = new Hability[numberOfHabilities];
        for (int nHability = 0; nHability < tHabilities.Length; nHability++)
        {
            Hability hability = (Hability)gameObject.AddComponent(tHabilities[nHability]);
            hability.enabled = isLocalPlayer;
            hability.Initialize(nHability);
            habilities[nHability] = hability;
        }
        if ( isLocalPlayer )
            Component.FindObjectOfType<UI_HabilityManager>().OnSetPlayerHabilities(gameObject);
    }

    [PunRPC]
    void ExecuteHability(int nHability)
    {
        habilities[nHability].ExecuteHability();
    }

    void OnDestroy()
    {
        try
        {
            int playerID = (int)photonView.instantiationData[0];
            bool isLocalPlayer = playerID == PhotonNetwork.player.ID;
            if ( isLocalPlayer )
                Component.FindObjectOfType<UI_HabilityManager>().OnPlayerDeath(gameObject);
        }
        catch { }
    }
}
