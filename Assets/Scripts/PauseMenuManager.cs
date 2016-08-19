using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class PauseMenuManager : MonoBehaviour {

    public static PauseMenuManager pauseMenuManager;

    private GameObject pausePanel;

    MouseController mouseController;

    public static bool isPausePanelActive = false;

    void Awake()
    {
        if (pauseMenuManager == null)
            pauseMenuManager = this;
        else
            Debug.Log("There should be only one pause menu manager");
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
            mouseController.SetCursorHidden(isPausePanelActive);
            isPausePanelActive = !isPausePanelActive;
            pausePanel.SetActive(isPausePanelActive);
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
}
