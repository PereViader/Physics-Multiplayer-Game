using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ExitGameManager : MonoBehaviour {

    public void ExitGame()
    {
        if (PhotonNetwork.connectedAndReady)
            PhotonNetwork.LeaveRoom();
        else
            LoadMainMenu();
    }

    public void OnLeftRoom()
    {
        LoadMainMenu();
    }

    void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
