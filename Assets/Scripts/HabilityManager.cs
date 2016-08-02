using UnityEngine;
using System.Collections;

public class HabilityManager : MonoBehaviour {

    private MonoBehaviour hability1;
    private MonoBehaviour hability2;

	void Awake () {
        if (PhotonNetwork.isMasterClient)
            AddRandomHabilities();
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
        Debug.Log("Set Habilities");
        string habilityVirtualKey = "Hability" + 1;
        hability1 = AddHability(habilityVirtualKey,hability1Index);
        habilityVirtualKey = "Hability" + 2;
        hability2 = AddHability(habilityVirtualKey,hability2Index);
    }

    MonoBehaviour AddHability (string habilityVirtualKey, int hability)
    {
        MonoBehaviour c;
        switch (hability)
        {
            case 0:
                HabilityJump jump = gameObject.AddComponent<HabilityJump>();
                c = jump;
                jump.SetVirtualKey(habilityVirtualKey);
                jump.enabled = false;
                break;
            case 1:
                HabilityGuard guard = gameObject.AddComponent<HabilityGuard>();
                c = guard;
                guard.SetVirtualKey(habilityVirtualKey);
                guard.enabled = false;
                break;
            case 2:
            default:
                HabilityPush push = gameObject.AddComponent<HabilityPush>();
                c = push;
                push.SetVirtualKey(habilityVirtualKey);
                push.enabled = false;
                break;
        }
        return c;
    }

    public void ActivateInputCaptureForHabilities()
    {
        hability1.enabled = true;
        hability2.enabled = true;
    }
}
