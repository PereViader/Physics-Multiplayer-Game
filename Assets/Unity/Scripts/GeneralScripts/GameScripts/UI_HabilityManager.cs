using UnityEngine;
using System.Collections;

public class UI_HabilityManager : MonoBehaviour {

    [SerializeField]
    private GameObject habilityParent;

    [SerializeField]
    private UI_Hability[] habilities;

    private GameObject player;

    public void OnSetPlayerHabilities(GameObject player)
    {
        Hability[] playerHabilities = player.GetComponents<Hability>();
        habilityParent.SetActive(true);
        for (int i = 0; i < playerHabilities.Length; i++)
        {
            habilities[i].SetHability(playerHabilities[i]);
        }
        enabled = true;
    }

    public void OnPlayerDeath(GameObject player)
    {
        habilityParent.SetActive(false);
        enabled = false;
    }

    public void setDisplay(bool state)
    {
        habilityParent.SetActive(state);
    }

    void Update()
    {
        foreach (UI_Hability hability in habilities)
        {
            hability.Update();
        }
    }
}
