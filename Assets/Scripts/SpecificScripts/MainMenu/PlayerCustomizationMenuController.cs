using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerCustomizationMenuController : MonoBehaviour {

    [SerializeField]
    MeshRenderer displayDummy;

    Material[] aviableTextures;

    [SerializeField]
    RectTransform customizeButtonParent;

    void Start()
    {
        aviableTextures = Resources.LoadAll<Material>("PlayerTextures");
        InititializeCustomizeMenu(aviableTextures);
        SetStartingSkin();
    }

    [SerializeField]
    float buttonSize;

    void InititializeCustomizeMenu(Material[] materials)
    {     
        foreach ( Material material in materials)
        {
            GameObject gButton = (GameObject)Instantiate(Resources.Load("MainMenu/SkinButton"));
            gButton.transform.SetParent(customizeButtonParent,false);

            string name = material.name;
            gButton.GetComponentInChildren<Text>().text = name;

            UnityAction onClick = () => this.ChangePlayerMaterial(name);
            gButton.GetComponent<Button>().onClick.AddListener(onClick);

        }
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
