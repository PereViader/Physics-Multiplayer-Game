using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour {
    public static bool gameWon;
    public static int experience;

    Text endGameText;
    Text scoreText;

	void Awake () {
        scoreText = GameObject.Find("GameScore").GetComponent<Text>();
        endGameText = GameObject.Find("EndGameText").GetComponent<Text>();
        SetTitle(gameWon);
        SetScore(experience);
    }

    void SetScore(int score)
    {
        scoreText.text += score.ToString();
    }

    void SetTitle(bool isWinner)
    {
        if ( isWinner )
        {
            endGameText.text = "You Win";
        } else
        {
            endGameText.text = "You Lose";
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
