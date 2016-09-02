using UnityEngine;
using System.Collections;

public class PlayerCustomizationMenuController : MonoBehaviour {

    [SerializeField]
    MeshRenderer displayDummy;

    Material[] aviableTextures;

    void Start()
    {
        aviableTextures = Resources.LoadAll<Material>("PlayerTextures");
        SetStartingSkin();
    }

    void SetStartingSkin()
    {
        string startingSkinName = GetStartingSkinName();
        Material startingSkin = FindMaterialByName(startingSkinName);
        ChangeDummySkin(startingSkin);
        SetPhotonPlayerSkin(startingSkinName);
    }

    

    void ChangeDummySkin(Material newSkin)
    {
        displayDummy.material = newSkin;
    }

    string GetStartingSkinName()
    {
        string startingSkinName = PlayerPrefs.GetString("Skin");
        if (startingSkinName == "")
            startingSkinName = "DefaultMaterial";
        return startingSkinName;
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

    void SetPhotonPlayerSkin(string skin)
    {
        PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "Skin", skin } });
    }

    private Material FindMaterialByName(string name)
    {
        Material texture = null;
        foreach (Material t in aviableTextures)
            if (t.name == name)
                texture = t;
        return texture;
    }

    void OnGUI()
    {
        try
        {
            foreach (var entry in PhotonNetwork.player.customProperties)
            {
                GUILayout.Label(entry.Key + " - " + entry.Value);
            }
        }
        catch (System.Exception) { Debug.Log("error"); }
    }
}
