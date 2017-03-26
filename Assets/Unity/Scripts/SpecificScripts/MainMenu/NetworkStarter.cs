using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkStarter : MonoBehaviour {

    [SerializeField]
    private GameObject menu;

    [SerializeField]
    private GameObject networkConnectingPanel;

    [SerializeField]
    private Text message;

    [SerializeField]
    private GameObject wait;

    [SerializeField]
    private string onFailedToConnect;

    [SerializeField]
    private float timeToCloseOnFailToConnect;

    void Awake()
    {
        if (!PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings(GamePreferences.GAME_VERSION);
    }

    void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        wait.SetActive(false);
        message.text = onFailedToConnect;
        Invoke("CloseGame", timeToCloseOnFailToConnect);
    }

    void CloseGame()
    {
        Debug.Log("Closing game after network fail to connect");
        GetComponent<MenuManager>().OnExitButtonPressed();
    }

    void Update()
    {
        if (PhotonNetwork.connectedAndReady)
        {
            menu.SetActive(true);
            GameObject.Destroy(networkConnectingPanel);
            Destroy(this);
        }
    }
}
