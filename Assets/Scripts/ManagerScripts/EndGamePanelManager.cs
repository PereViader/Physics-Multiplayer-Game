using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndGamePanelManager : MonoBehaviour {

    /*Text winLoseText;
    Text playerExperienceText;*/

    void Awake()
    {
        /*Transform endGamePanel = GameObject.Find("Canvas").transform.Find("EndGamePanel");
        winLoseText = endGamePanel.Find("WinLoseText").GetComponent<Text>();
        playerExperienceText = endGamePanel.Find("ExperienceText").GetComponent<Text>();*/
    }
    
    public void SetScoreAndEndGame(int score, int teamWon)
    {
        /*
        int previousExperience = PlayerExperience.GetExperience();
        int newExperience = PlayerExperience.AddExperience(score);
        playerExperienceText.text += previousExperience.ToString() + " -> " + newExperience.ToString();
        int playerTeam = (int)PhotonNetwork.player.customProperties["Team"];
        winLoseText.gameObject.transform.parent.gameObject.SetActive(true);
        if (teamWon == playerTeam)
        {
            winLoseText.text = "You Win";
        }
        else
        {
            winLoseText.text = "You Lose";
        }
        */
    }
}
