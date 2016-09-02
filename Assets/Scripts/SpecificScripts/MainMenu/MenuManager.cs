using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour {

    [SerializeField]
    private GameObject optionsPanel;

    [SerializeField]
    private GameObject playPanel;

    [SerializeField]
    private GameObject customizationPanel;


    public void OnExitButtonPressed()
    {
        Application.Quit();
    }

    public void OnOptionsButtonPressed()
    {
        optionsPanel.SetActive(true);
    }

    public void OnOptionsBackButtonPressed()
    {
        optionsPanel.SetActive(false);
    }

    public void OnCustomizeButtonPressed()
    {
        customizationPanel.SetActive(true);
    }

    public void OnCustomizeBackButtonPressed()
    {
        customizationPanel.SetActive(false);
    }

    public void OnGameModeButtonPressed()
    {
        playPanel.SetActive(true);
    }

    public void OnGameModeBackButtonPressed()
    {
        playPanel.SetActive(false);
    }
}
