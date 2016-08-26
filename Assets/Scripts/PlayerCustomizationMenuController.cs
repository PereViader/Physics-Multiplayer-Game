using UnityEngine;
using System.Collections;

public class PlayerCustomizationMenuController : MonoBehaviour {
    [SerializeField]
    Material[] aviableTextures;

    [SerializeField]
    MeshRenderer displayDummy;

    void Awake()
    {
        SetStartingSkin();
    }

    void SetStartingSkin()
    {
        Material startingSkin = GetStartingSkin();
        if (startingSkin == null)
        {
            Debug.Log("Starting skin not set");
        }
        else
        {
            ChangeDummySkin(startingSkin);
        }
    }

    void SetPhotonPlayerSkin(string skin)
    {
        PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "Skin", skin } });
    }

    void ChangeDummySkin(Material newSkin)
    {
        displayDummy.material = newSkin;
    }

    Material GetStartingSkin()
    {
        Material startingSkin;
        string startingSkinName = PlayerPrefs.GetString("Skin");
        if (startingSkinName != "")
        {
            startingSkin = FindMaterialByName(startingSkinName);
        }
        else
        {
            startingSkin = null;
        }
        return startingSkin;
    }

    public void ChangePlayerMaterial(string newMaterialName)
    {
        Material newMaterial = FindMaterialByName(newMaterialName);
        if (newMaterial == null)
            Debug.Log("Invalid Material Name");
        else
        {
            ChangeDummySkin(newMaterial);
            PlayerPrefs.SetString("Skin", newMaterialName);
            SetPhotonPlayerSkin(newMaterialName);
        }
    }

    private Material FindMaterialByName(string name)
    {
        Material texture = null;
        foreach (Material t in aviableTextures)
            if (t.name == name)
                texture = t;
        return texture;
    }


}
