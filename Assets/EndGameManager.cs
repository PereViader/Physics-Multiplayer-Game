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
        PhotonNetwork.automaticallySyncScene = false;
        PhotonNetwork.LeaveRoom();

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
