using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu_ConnectNetwork : MonoBehaviour {

    [SerializeField]
    MainMenu_MainManager menuManager;

    [SerializeField]
    private Text mainMessage;

    [SerializeField]
    private Text subMessage;

    [SerializeField]
    private string onFailedToConnectMessage;

    [SerializeField]
    private string OnConnectionFailedMessage;

    [SerializeField]
    private float timeToCloseOnFailToConnect;

    void Awake()
    {
        Debug.Log("Awake connected" + PhotonNetwork.connected);
        if (!PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings(GamePreferences.GAME_VERSION);
        else
            OnConnectedToMaster();
    }

    void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        mainMessage.text = onFailedToConnectMessage;
        subMessage.text = cause.ToString();
        Invoke("CloseGame", timeToCloseOnFailToConnect);
    }

    void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        menuManager.OpenMenu();
    }

    void OnConnectionFail(DisconnectCause cause)
    {
        menuManager.OpenLoadingMenu();
        mainMessage.text = OnConnectionFailedMessage;
        subMessage.text = cause.ToString();
        Invoke("CloseGame", timeToCloseOnFailToConnect);
    }

    void CloseGame()
    {
        GetComponent<MainMenu_MainManager>().OnExitButtonPressed();
    }
}
