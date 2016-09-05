using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class PauseMenuManager : MonoBehaviour {

    GameObject pausePanel;
    ExitGameManager exitGameManager;
    bool isPausePanelActive;

    void Awake()
    {
        pausePanel = GameObject.Find("Canvas").transform.Find("PauseUI").gameObject;
        exitGameManager = Component.FindObjectOfType<ExitGameManager>();
        InputState.ActivateGameInput();
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            isPausePanelActive = !isPausePanelActive;
            if (isPausePanelActive)
                InputState.ActivateMenuInput();
            else
                InputState.ActivateGameInput();
            pausePanel.SetActive(isPausePanelActive);
        }
    }

    public void OnMainMenuButtonPressed()
    {
        Debug.Log("Main Menu Button pressed");
        exitGameManager.ExitGame();
    }

    
}
