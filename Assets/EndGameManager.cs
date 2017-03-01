using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour {

    [SerializeField]
    private Text endGameText;

    public static int experienceGained;
    public static PlayerProperties.GameResult gameResult;

    void Start()
    {
        InputState.ActivateMenuInput();
        SetTitle(gameResult);
        GetComponent<ExperienceBarManager>().AddExperienceAndAnimate(experienceGained);
        ResetEndGameState();
    }

    private void ResetEndGameState()
    {
        experienceGained = 0;
        gameResult = PlayerProperties.GameResult.None;
    }



    /*
	void Awake () {
        InputState.ActivateMenuInput();
        if (!PhotonNetwork.isMasterClient)
            PhotonNetwork.LeaveRoom();
        scoreText = GameObject.Find("GameScore").GetComponent<Text>();
        endGameText = GameObject.Find("EndGameText").GetComponent<Text>();
        PhotonPlayer player = PhotonNetwork.player;
        PlayerProperties.GameResult result = PlayerProperties.GameResult.None;
        object oResult;
        if (player.customProperties.TryGetValue(PlayerProperties.gameResult, out oResult))
        {
            result = (PlayerProperties.GameResult)oResult;
        }
        int experience = 0;
        object oExperience;
        if (player.customProperties.TryGetValue(PlayerProperties.experience, out oExperience))
        {
            experience = (int)oExperience;
        }
        player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { PlayerProperties.gameResult, null }, { PlayerProperties.experience, null } });
        
        SetTitle(result);
        SetScore(experience);
        GetComponent<ExperienceBarManager>().AddExperienceAndAnimate(experience);
    }

    void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        if (PhotonNetwork.isMasterClient && PhotonNetwork.playerList.Length == 1)
            PhotonNetwork.LeaveRoom();
    }*/

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
