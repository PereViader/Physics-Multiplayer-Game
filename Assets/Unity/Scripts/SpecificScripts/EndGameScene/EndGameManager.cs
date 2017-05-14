using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour {

    [SerializeField]
    private Text endGameMessage;

    [SerializeField]
    private string loseMessage;

    [SerializeField]
    private string winMessage;

    [SerializeField]
    private string tieMessage;

    public static int experienceGained;
    public static PlayerProperties.GameResult gameResult;

    void Start()
    {
        PhotonNetwork.automaticallySyncScene = false;
        PhotonNetwork.LeaveRoom();

        InputState.ActivateMenuInput();
        SetMessage(gameResult);
        GetComponent<ExperienceBarManager>().AddExperienceAndAnimate(experienceGained);
        ResetEndGameState();
    }

    private void ResetEndGameState()
    {
        experienceGained = 0;
        gameResult = PlayerProperties.GameResult.None;
    }

    private void SetMessage(PlayerProperties.GameResult result)
    {
        switch(result)
        {
            case PlayerProperties.GameResult.None:
                endGameMessage.text = "None maybe error?";
                break;
            case PlayerProperties.GameResult.Lose:
                endGameMessage.text = loseMessage;
                break;
            case PlayerProperties.GameResult.Tie:
                endGameMessage.text = tieMessage;
                break;
            case PlayerProperties.GameResult.Win:
                endGameMessage.text = winMessage;
                break;
        }
    }

    public void OnMainMenuButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
