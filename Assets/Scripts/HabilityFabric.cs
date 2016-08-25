using UnityEngine;
using System.Collections;
using System;

public class HabilityFabric{

    public static void FillWithRandomHabilityIndex(ref int[] habilities)
    {
        for (int i = 0; i < habilities.Length; i++)
            habilities[i] = -1;

        for (int i = 0; i < habilities.Length; i++)
        {
            int newHability;
            do
            {
                newHability = GetRandomHability();
            } while (Array.IndexOf(habilities, newHability) >= 0);
            habilities[i] = newHability;
        }
    }

    public static int GetRandomHability()
    {
        return UnityEngine.Random.Range(0, 5);
    }

    public static Type GethabilityType(int habilityNumber)
    {
        switch (habilityNumber)
        {
            case 0:
                return typeof(HabilityJump);
            case 1:
                return typeof(HabilityGuard);
            case 2:
                return typeof(HabilityPush);
            case 3:
                return typeof(HabilityShrink);
            case 4:
            default:
                return typeof(HabilityStop);
        }
    }
}
