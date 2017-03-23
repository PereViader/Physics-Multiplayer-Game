using UnityEngine;
using System.Collections;
using System;

public class KingOfTheHill_MapManager : MonoBehaviour, IGame {

    [SerializeField]
    private MapMatrixGenerator mapMatrixGenerator;

    [SerializeField]
    private MapMeshGenerator mapMeshGenerator;

    [SerializeField]
    private float exteriorBounds;

    [SerializeField]
    private GameObject map;

    [SerializeField]
    private GameObject deadZone;

    [SerializeField]
    private Transform ceiling;

    [SerializeField]
    private Transform northWall;

    [SerializeField]
    private Transform southWall;

    [SerializeField]
    private Transform eastWall;

    [SerializeField]
    private Transform westWall;


    public void OnGameSetup() { }

    public void OnGameStart() { }

    public void OnRoundSetup()
    {
        prepareNewMap();
    }

    public void OnRoundStart() { }

    public void OnRoundEnd() { }

    public void OnGameEnd() { }


    public void prepareNewMap()
    {
        ExitGames.Client.Photon.Hashtable roomProperties = PhotonNetwork.room.customProperties;
        roomProperties["currentMapSeed"] = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        PhotonNetwork.room.SetCustomProperties(roomProperties);
        GetComponent<PhotonView>().RPC("createNewMap", PhotonTargets.All);
    }

    [PunRPC]
    void createNewMap()
    {
        bool[,] mapMatrix = mapMatrixGenerator.GenerateMatrix((int)PhotonNetwork.room.customProperties["currentMapSeed"]);
        Mesh mapMesh = mapMeshGenerator.GenerateGeometryFromMesh(mapMatrix);

        MeshFilter meshFilter = map.GetComponent<MeshFilter>();
        MeshCollider meshCollider = map.GetComponent<MeshCollider>();

        // unity recomana posar a null primer ja que internament fa unes optimitzacions
        meshFilter.mesh = null;
        meshCollider.sharedMesh = null;
        meshFilter.mesh = mapMesh;
        meshCollider.sharedMesh = mapMesh;


        Vector3 floorCeilingScale = new Vector3(
                    (mapMesh.bounds.extents.x + exteriorBounds) * 2,
                    (mapMesh.bounds.extents.z + exteriorBounds) * 2,
                    1);
        deadZone.transform.localScale = floorCeilingScale;
        deadZone.transform.position = new Vector3(0, -mapMeshGenerator.sideHeight, 0);

        ceiling.position = new Vector3(0, 20 - mapMeshGenerator.sideHeight, 0);
        ceiling.localScale = floorCeilingScale;

        Vector3 wallScalex = new Vector3((mapMesh.bounds.extents.x+exteriorBounds) * 2, 20, 1);
        northWall.localScale = wallScalex;
        southWall.localScale = wallScalex;
        Vector3 wallScalez = new Vector3((mapMesh.bounds.extents.z + exteriorBounds) * 2, 20, 1);

        eastWall.localScale = wallScalez;
        westWall.localScale = wallScalez;

        northWall.position = new Vector3(0, 10 - mapMeshGenerator.sideHeight, mapMesh.bounds.extents.z + exteriorBounds);
        southWall.position = new Vector3(0, 10 - mapMeshGenerator.sideHeight, -mapMesh.bounds.extents.z - exteriorBounds);
        eastWall.position = new Vector3(mapMesh.bounds.extents.x + exteriorBounds, 10 - mapMeshGenerator.sideHeight, 0);
        westWall.position = new Vector3(-mapMesh.bounds.extents.x - exteriorBounds, 10 - mapMeshGenerator.sideHeight, 0);
    }
}
