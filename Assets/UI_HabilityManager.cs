using UnityEngine;
using System.Collections;

public class UI_HabilityManager : MonoBehaviour {

    [SerializeField]
    private GameObject habilityParent;

    [SerializeField]
    private UI_Hability[] hability;
	
	public UI_Hability getHability(int habilityNum)
    {
        return hability[habilityNum];
    }

    public void setDisplay(bool state)
    {
        habilityParent.SetActive(state);
    }
}
