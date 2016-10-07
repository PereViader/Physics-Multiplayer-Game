﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour {

    [SerializeField]
    GameObject menuPanel;

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