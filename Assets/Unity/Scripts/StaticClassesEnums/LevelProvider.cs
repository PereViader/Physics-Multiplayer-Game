using UnityEngine;
using System.Collections.Generic;

public class LevelProvider {
    public static GameMode GetRandomGameMode()
    {
        return (GameMode)Random.Range(0, System.Enum.GetNames(typeof(GameMode)).Length);
    }

    public static string GetRandomMap(GameMode gameMode)
    {
        TextAsset textAsset = Resources.Load<TextAsset>("GameModeScenes/" + gameMode.ToString()+"/Scenes");
        string[] scenes = textAsset.text.Split('\n');
        int chosenMapIndex = Random.Range(0, scenes.Length);
        return scenes[chosenMapIndex];
    }

    public static string GetRandomMap()
    {
        GameMode gameMode = GetRandomGameMode();
        return GetRandomMap(gameMode);
    } 

}
