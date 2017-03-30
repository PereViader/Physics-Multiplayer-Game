using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class UI_LeaderBoardManager : MonoBehaviour {

    [SerializeField]
    private Transform playerParent;

    [SerializeField]
    private GameObject playerLabelPrefab;

    void Update()
    {
        try
        {
            if (PhotonNetwork.playerList.Length > 0)
            {
                while (playerParent.childCount > PhotonNetwork.playerList.Length)
                {
                    Destroy(playerParent.GetChild(0));
                }

                while (playerParent.childCount < PhotonNetwork.playerList.Length)
                {
                    Instantiate(playerLabelPrefab, playerParent);
                }

                for (int nPlayer = 0; nPlayer < PhotonNetwork.playerList.Length; nPlayer++)
                {
                    PhotonPlayer player = PhotonNetwork.playerList[nPlayer];
                    string playerName = player.name;
                    int playerScore = (int)player.customProperties[PlayerProperties.score];
                    playerParent.GetChild(nPlayer).GetComponent<Text>().text = playerName + ": " + playerScore;
                }
            }
        } catch { }
    }
}
