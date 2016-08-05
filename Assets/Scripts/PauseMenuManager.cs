﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenuManager : MonoBehaviour {


    [SerializeField]
    private GameObject pausePanel;

    private PlayerControllerPast playerController;
    private CameraFollow cameraController;

    private bool isPausePanelVisible = false;

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (playerController != null) {
                playerController.SetInput(isPausePanelVisible);
                cameraController.SetInput(isPausePanelVisible);
                isPausePanelVisible = !isPausePanelVisible;
                pausePanel.SetActive(isPausePanelVisible);
            }
        }
    }

    public void OnMainMenuButtonPressed()
    {
        PhotonNetwork.automaticallySyncScene = false;
        PhotonNetwork.LeaveRoom();
    }

    public void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel(0);
    }

    public void SetPlayer(PlayerControllerPast player)
    {
        playerController = player;
    }

    public void SetCamera(CameraFollow camera)
    {
        cameraController = camera;
    }
}
