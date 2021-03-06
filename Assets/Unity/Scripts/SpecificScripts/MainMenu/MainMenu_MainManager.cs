﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public class MainMenu_MainManager : MonoBehaviour {

    [SerializeField]
    private GameObject loadingPanel;

    [SerializeField]
    private GameObject menuPanel;

    [SerializeField]
    private GameObject optionsPanel;

    [SerializeField]
    private GameObject playPanel;

    [SerializeField]
    private GameObject customizationPanel;

    //EventSystem eventSystem;

    void Awake()
    {
        //eventSystem = Component.FindObjectOfType<EventSystem>();
    }



    public void OnExitButtonPressed()
    {
        if (PhotonNetwork.connected)
            PhotonNetwork.Disconnect();
        else
            OnDisconnectedFromPhoton();
    }

    public void OnDisconnectedFromPhoton()
    {
        Debug.Log("disconnect");
        Application.Quit();
    }

    public void OpenMenu()
    {
        menuPanel.SetActive(true);
        loadingPanel.SetActive(false);
    }

    public void OpenLoadingMenu()
    {
        menuPanel.SetActive(false);
        optionsPanel.SetActive(false);
        customizationPanel.SetActive(false);
        playPanel.SetActive(false);
        loadingPanel.SetActive(true);
    }

    //TODO fer que cada vegada que s'entra i es surt de una finestra es canvii l'objecte actiu

    public void OnOptionsButtonPressed()
    {
        menuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void OnOptionsBackButtonPressed()
    {
        menuPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void OnCustomizeButtonPressed()
    {
        menuPanel.SetActive(false);
        customizationPanel.SetActive(true);
    }

    public void OnCustomizeBackButtonPressed()
    {
        menuPanel.SetActive(true);
        customizationPanel.SetActive(false);
    }

    public void OnGameModeButtonPressed()
    {
        menuPanel.SetActive(false);
        playPanel.SetActive(true);
    }

    public void OnGameModeBackButtonPressed()
    {
        menuPanel.SetActive(true);
        playPanel.SetActive(false);
    }
}
