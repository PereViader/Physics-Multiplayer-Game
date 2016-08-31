using UnityEngine;
using System.Collections;

public class PlayerProperties : MonoBehaviour {
    public static string team = "Team";
    public static string experience = "Experience";
    public static string gameResult = "Result";
    public static string skin = "Skin";

    public enum GameResult
    {
        None,
        Lose,
        Win,
        Tie
    }
}
