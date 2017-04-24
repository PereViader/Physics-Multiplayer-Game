using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class HabilityFabric{

    private static Type[] AVIABLE_HABILITIES = new Type[]{ typeof(HabilityJump), typeof(HabilityGuard), typeof(HabilityShrink), typeof(HabilityStop) };

    public static Type[] GenerateHabilitiesFromSeed(int seed, int nHabilities)
    {
        System.Random random = new System.Random(seed);
        List<Type> habilities = new List<Type>();
        while (habilities.Count < nHabilities)
        {
            Type type = GenerateRandomHability(random);
            if (!habilities.Contains(type))
                habilities.Add(type);
        }
        return habilities.ToArray();
    }

    public static Type GenerateRandomHability(System.Random r)
    {
        int randomIndex = r.Next(0,AVIABLE_HABILITIES.Length);
        return AVIABLE_HABILITIES[randomIndex];
    }
}
