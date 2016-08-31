using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour {
    Text endGameText;
    Text scoreText;

	void Awake () {
        if (!PhotonNetwork.isMasterClient)
            PhotonNetwork.LeaveRoom();
        scoreText = GameObject.Find("GameScore").GetComponent<Text>();
        endGameText = GameObject.Find("EndGameText").GetComponent<Text>();
        PhotonPlayer player = PhotonNetwork.player;
        PlayerProperties.GameResult result = (PlayerProperties.GameResult)player.customProperties[PlayerProperties.gameResult];
        int experience = (int)player.customProperties[PlayerProperties.experience];
        player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { PlayerProperties.gameResult, null }, { PlayerProperties.experience, null } });
        
        SetTitle(result);
        SetScore(experience);
        GetComponent<ExperienceBarManager>().AddExperienceAndAnimate(experience);
    }

    void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        if (PhotonNetwork.isMasterClient && PhotonNetwork.playerList.Length == 1)
            PhotonNetwork.LeaveRoom();
    }

    void SetScore(int score)
    {
        scoreText.text += score.ToString();
    }

    void SetTitle(PlayerProperties.GameResult result)
    {
        switch(result)
        {
            case PlayerProperties.GameResult.None:
                endGameText.text = "None maybe error?";
                break;
            case PlayerProperties.GameResult.Lose:
                endGameText.text = "You Lose";
                break;
            case PlayerProperties.GameResult.Tie:
                endGameText.text = "It's a Tie";
                break;
            case PlayerProperties.GameResult.Win:
                endGameText.text = "You Win";
                break;
        }
    }

    public void OnMainMenuButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
