using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CaptureUI_HabilityManager : MonoBehaviour {

    GameObject habilityParent;

    Text[] habilityName;
    Text[] habilityCooldown;

    void Awake()
    {
        habilityParent = GameObject.Find("Canvas/GameUI").transform.Find("HabilitiesUI").gameObject;
        int numberOfHabilities = habilityParent.transform.childCount;
        habilityName = new Text[numberOfHabilities];
        habilityCooldown = new Text[numberOfHabilities];
        for ( int i = 0; i < numberOfHabilities; i++ )
        {
            habilityName[i] = habilityParent.transform.GetChild(i).Find("Name").GetComponent<Text>();
            habilityCooldown[i] = habilityParent.transform.GetChild(i).Find("Cooldown").GetComponent<Text>();
        }
    }

    public void SetActive(bool state)
    {
        if ( habilityParent )
            habilityParent.SetActive(state);
    }

    public void SetName(int hability, string name)
    {
        habilityName[hability].text = name;
    }

    public void SetCooldown(int hability, float cooldown)
    {
        habilityCooldown[hability].text = cooldown.ToString("0.0");
    }
}
