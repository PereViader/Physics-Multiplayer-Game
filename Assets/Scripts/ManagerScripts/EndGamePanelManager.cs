using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndGamePanelManager : MonoBehaviour {

    Text winLoseText;

    void Awake()
    {
        CaptureEvents.OnGameEnded += OnGameEnded;
        winLoseText = GameObject.Find("Canvas").transform.Find("EndGamePanel/WinLoseText").GetComponent<Text>();
    }

    void OnDestroy()
    {
        CaptureEvents.OnGameEnded -= OnGameEnded;
    }

    void OnGameEnded(int teamWon)
    {
        int playerTeam = (int)PhotonNetwork.player.customProperties["Team"];
        Time.timeScale = 0;
        winLoseText.gameObject.transform.parent.gameObject.SetActive(true);
        if ( teamWon == playerTeam)
        {
            winLoseText.text = "You Win";
        } else
        {
            winLoseText.text = "You Lose";
        }
    }
}
