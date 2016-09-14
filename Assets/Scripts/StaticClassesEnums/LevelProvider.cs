using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class LevelProvider {
    public static GameMode GetRandomGameMode()
    {
        return (GameMode)Random.Range(0, System.Enum.GetNames(typeof(GameMode)).Length);
    }

    public static string GetRandomMap(GameMode gameMode)
    {
        SceneAsset[] gameModeMaps = Resources.LoadAll<SceneAsset>("GameModeScenes/" + gameMode.ToString());
        int chosenMapIndex = Random.Range(0, gameModeMaps.Length);
        return gameModeMaps[chosenMapIndex].name;
    }

    public static string GetRandomMap()
    {
        GameMode gameMode = GetRandomGameMode();
        return GetRandomMap(gameMode);
    } 

}
