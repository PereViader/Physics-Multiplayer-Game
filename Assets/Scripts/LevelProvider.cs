using UnityEngine;

public class LevelProvider {

    private const int minCaptureBuildIndex = 1;
    private const int maxCaptureBuildIndex = 1;

    private const int minBombBuildIndex = 3;
    private const int maxBombBuildIndex = 3;

    public static int GetRandomCaptureMap()
    {
        return Random.Range(minCaptureBuildIndex, maxCaptureBuildIndex+1);
    }

    public static int GetRandomBombMap()
    {
        return Random.Range(minBombBuildIndex, maxBombBuildIndex + 1);
    }

    public static int GetRandomIAMap()
    {
        throw new System.Exception("Not implemented");
    }

    public static int GetRandomMap(int gameMode)
    {
        Random.seed = (int)System.DateTime.Now.Ticks;
        int ret = -1;
        switch (gameMode)
        {
            case GameMode.Capture:
                ret = GetRandomCaptureMap();
                break;
            case GameMode.Bomb:
                ret = GetRandomBombMap();
                break;
            case GameMode.IA:
                ret = GetRandomIAMap();
                break;
            default:
                throw new System.Exception("No gamplay associated with " + gameMode);
        }
        return ret;
    }

}
