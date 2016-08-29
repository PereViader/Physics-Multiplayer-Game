using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour {
    public enum GameResult
    {
        Lose,
        Win,
        Tie
    }

    public static GameResult result;
    public static int experience;

    Text endGameText;
    Text scoreText;

	void Awake () {
        result = GameResult.Win;
        experience = 3000;
        scoreText = GameObject.Find("GameScore").GetComponent<Text>();
        endGameText = GameObject.Find("EndGameText").GetComponent<Text>();
        SetTitle(result);
        SetScore(experience);
    }

    void SetScore(int score)
    {
        scoreText.text += score.ToString();
    }

    void SetTitle(GameResult result)
    {
        switch(result)
        {
            case GameResult.Lose:
                endGameText.text = "You Lose";
                break;
            case GameResult.Tie:
                endGameText.text = "It's a Tie";
                break;
            case GameResult.Win:
                endGameText.text = "You Win";
                break;
        }
    }

    void Start()
    {
        GetComponent<ExperienceBarManager>().AddExperienceAndAnimate(experience);
    }

    public void OnMainMenuButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
