using UnityEngine;
using System.Collections;

public class PlayerSpawner {

    Transform[][] spawns;

	public PlayerSpawner(int teams)
    {
        spawns = new Transform[teams][];
        GameObject container;

        for( int i = 0; i<teams; i++)
        {
            container = GameObject.Find("Map/SpawnPositions/Team"+(i+1));
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
}
