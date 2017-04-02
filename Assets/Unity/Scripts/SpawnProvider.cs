using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpawnProvider : MonoBehaviour {
    [SerializeField]
    private Transform spawnParent;

    private Transform[] spawns;

    void Awake()
    {
        spawns = new Transform[spawnParent.childCount];
        for (int child = 0; child < spawnParent.childCount; child++)
        {
            spawns[child] = spawnParent.GetChild(child);
        }
    }

    public void UpdateSpawns(List<Vector3> newSpawnsVector3)
    {

        foreach (Transform spawn in spawns)
            Destroy(spawn.gameObject);
        

        List<Transform> newSpawns = new List<Transform>();
        foreach (Vector3 spawnPosition in newSpawnsVector3)
        {
            GameObject spawn = new GameObject();
            spawn.transform.position = spawnPosition;
            spawn.transform.parent = spawnParent;
            newSpawns.Add(spawn.transform);
        }
        this.spawns = newSpawns.ToArray();
    }

    public Transform GetFreeSpawn()
    {
        Transform spawn;
        do
        {
            spawn = spawns[Random.Range(0, spawns.Length)];
        } while (PlayerInsideSpawn(spawn));
        return spawn;
    }

    private bool PlayerInsideSpawn(Transform spawn)
    {

        Collider[] colliders = Physics.OverlapSphere(spawn.position, 1f);    
        return colliders.Any(collider => collider.gameObject.tag == "Player");
    }
}
