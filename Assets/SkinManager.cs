using UnityEngine;
using System.Collections;

public class SkinManager : MonoBehaviour {

    void Awake()
    {
        GetComponent<PhotonPlayerOwner>().OnPlayerOwnerSet += PlayerOwnerSet; // no fa falta falta desubscriure's ja que quan aquest objecte s'elimina l'altre també i no queden referencies
    }

    void PlayerOwnerSet()
    {
        string playerSkin = (string)GetComponent<PhotonPlayerOwner>().GetOwner().customProperties["Skin"];
        if (playerSkin != "")
        {
            GetComponent<MeshRenderer>().material = (Material)Resources.Load(playerSkin);
        }
    }
}
