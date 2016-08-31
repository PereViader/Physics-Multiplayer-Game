using UnityEngine;
using System.Collections;

public class Capture_SpawnManager : MonoBehaviour {

    [SerializeField]
    int teamsInGame;

    Transform[][] spawns;

    void Awake()
    {
        spawns = new Transform[teamsInGame][];
        GameObject container;

        for (int i = 0; i < teamsInGame; i++)
        {
            container = GameObject.Find("Map/SpawnPositions/Team" + i);
            spawns[i] = new Transform[container.transform.childCount];
            for (int j = 0; j < container.transform.childCount; j++)
            {
                spawns[i][j] = container.transform.GetChild(j);
            }
        }
    }

    public Transform GetSpawn(int team, int spawnIndex)
    {
        return spawns[team][spawnIndex];
    }

    public int GetRandomSpawnIndex(int team)
    {
        return Random.Range(0, spawns[team].Length);
    }

    public Transform GetRandomSpawn(int team)
    {
        return spawns[team][GetRandomSpawnIndex(team)];
    }
}
