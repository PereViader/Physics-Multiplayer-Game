using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSkinLoader {

    public static void Initialize(int level)
    {
        PlayerExperience.SetLevel(level);
    }

    public static Material[] LoadPlayerSkins()
    {
        int playerLevel = PlayerExperience.GetLevel();
        Material[] aviableSkins = Resources.LoadAll<Material>("PlayerTextures");
        List<Material> skins = new List<Material>();
        foreach(Material skin in aviableSkins)
        {
            int skinLevel = int.Parse(skin.name.Split('.')[0]);
            if (skinLevel <= playerLevel)
            {
                skins.Add(skin);
            }
        }
        return skins.ToArray();
    }
}
