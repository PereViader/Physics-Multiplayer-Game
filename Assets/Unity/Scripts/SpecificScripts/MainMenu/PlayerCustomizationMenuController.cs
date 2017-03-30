using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerCustomizationMenuController : MonoBehaviour {

    [SerializeField]
    MeshRenderer displayDummy;

    Material[] playerSkins;

    [SerializeField]
    RectTransform customizeButtonParent;    

    void Start()
    {
        playerSkins = PlayerSkinLoader.LoadPlayerSkins();
        InititializeCustomizeMenu(playerSkins);
        InitializeNickname();
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
        Material startingSkin = GetStartingSkin();
        ChangeDummySkin(startingSkin);
        SetPhotonPlayerSkin(startingSkin.name);
    }

    void ChangeDummySkin(Material newSkin)
    {
        displayDummy.material = newSkin;
    }

    Material GetStartingSkin()
    {
        string startingSkinName = PlayerPrefs.GetString("Skin");
        if (startingSkinName == "")
            startingSkinName = "0. Default Skin";
        return FindMaterialByName(startingSkinName);
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

    Material FindMaterialByName(string name)
    {
        Material texture = null;
        foreach (Material t in playerSkins)
            if (t.name == name)
                texture = t;
        return texture;
    }

    void InitializeNickname()
    {
        string nickname = PlayerPrefs.GetString("Nickname");
        if ( nickname == "" )
        {
            nickname = "Player" + Random.Range(0, 1000).ToString();
        }
        ChangeNickname(nickname);
    }

    public void ChangeNickname(string nickname)
    {
        PhotonNetwork.playerName = nickname;
        PlayerPrefs.SetString("Nickname", nickname);

    }

    void OnGUI()
    {
        try
        {
            foreach (var entry in PhotonNetwork.player.customProperties)
            {
                GUILayout.Label(entry.Key + " - " + entry.Value);
                GUILayout.Label("Level: " + PlayerExperience.GetLevel());
                GUILayout.Label("Nickname: " + PhotonNetwork.player.name);
            }
        }
        catch (System.Exception) { Debug.Log("error"); }
    }
}
