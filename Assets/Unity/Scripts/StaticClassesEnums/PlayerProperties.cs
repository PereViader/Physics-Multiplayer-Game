using UnityEngine;
using System.Collections;

public class PlayerProperties {
    public static readonly string team = "Team";
    public static readonly string experience = "Experience";
    public static readonly string gameResult = "Result";
    public static readonly string skin = "Skin";
    public static readonly string score = "score";

    public enum GameResult
    {
        None,
        Lose,
        Win,
        Tie
    }
}
