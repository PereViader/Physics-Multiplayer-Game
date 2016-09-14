using UnityEngine;
using System.Collections;

public class PlayerExperience {

    static int baseExperience = 400;
    static int incrementExperience = 200;

    public static int ExperienceToLevelUp(int level)
    {
        return baseExperience + incrementExperience * level;
    }

    public static int GetExperience()
    {
        return PlayerPrefs.GetInt("Experience");
    }

    public static float GetExperiencePercentageOfLevel()
    {
        int playerLevel = GetLevel();
        int playerExperience = GetExperience();
        int nextLevelExperience = ExperienceToLevelUp(playerLevel);
        return (float)playerExperience / nextLevelExperience;
    }

    public static int GetLevel()
    {
        return PlayerPrefs.GetInt("Level"); ;
    }

    public static void SetLevel(int level)
    {
        PlayerPrefs.SetInt("Level", level);
    }

    public static void SetExperience(int experience)
    {
        PlayerPrefs.SetInt("Experience", experience);
    }

    public static void AddExperience(int experience)
    {
        int playerLevel = GetLevel();
        int newExperience = GetExperience() + experience;
        int levelExperience = ExperienceToLevelUp(playerLevel);
        while(newExperience > levelExperience)
        {
            playerLevel += 1;
            newExperience -= levelExperience;
            levelExperience = ExperienceToLevelUp(playerLevel);
        }
        SetLevel(playerLevel);
        SetExperience(newExperience);
    }
}
