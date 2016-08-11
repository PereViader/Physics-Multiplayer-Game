using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class PauseMenuManager : MonoBehaviour {

    private GameObject pausePanel;

    MouseController mouseController;

    private PlayerControllerPast playerController;
    private CameraFollow cameraController;

    private bool isPausePanelVisible = false;

    void Awake()
    {
        mouseController = GetComponent<MouseController>();
        pausePanel = GameObject.Find("Canvas").transform.Find("PausePanel").gameObject;
        if (!pausePanel)
            Debug.Log("Pause Panel reference not found!");
        pausePanel.transform.Find("MainMenuButton").gameObject.GetComponent<Button>().onClick.AddListener(new UnityAction(OnMainMenuButtonPressed));
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (playerController != null) {
                playerController.SetInput(isPausePanelVisible);
                cameraController.SetInput(isPausePanelVisible);
                mouseController.SetCursorHidden(isPausePanelVisible);

                isPausePanelVisible = !isPausePanelVisible;
                pausePanel.SetActive(isPausePanelVisible);
            }
        }
    }

    public void OnMainMenuButtonPressed()
    {
        PhotonNetwork.automaticallySyncScene = false;
        PhotonNetwork.LeaveRoom();
        Debug.Log("Main Menu Button pressed");
    }

    public void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
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
