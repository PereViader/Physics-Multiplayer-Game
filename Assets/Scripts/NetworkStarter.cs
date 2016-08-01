using UnityEngine;
using System.Collections;

public class NetworkStarter : MonoBehaviour {

    [SerializeField]
    private GameObject menu;

    [SerializeField]
    private GameObject loading;

    void Awake()
    {
        if ( !PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings("1");
    }

    void Update()
    {
        if (PhotonNetwork.connectedAndReady)
        {
            menu.SetActive(true);
            GameObject.Destroy(loading);
            Destroy(this);
        }
    }
}
